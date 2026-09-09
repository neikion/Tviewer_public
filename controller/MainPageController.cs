using ImageMagick;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tviewer.Interfaces;
using Tviewer.model;
using Tviewer.model.DB;
using Tviewer.model.EventArgs;
using Tviewer.model.ImageData;
using Tviewer.model.SearchEngine;
using Tviewer.model.Setting;
using Tviewer.model.Util;
using Tviewer.view;

namespace Tviewer.controller
{
    public class MainPageController : ControllerBase
    {

        public enum PageState
        {
            Home,
            Search,
        }

        private ObservableBICollection<ImageListContent> _contentList = new ObservableBICollection<ImageListContent>();
        /// <summary>
        /// option category list
        /// </summary>
        public ObservableBICollection<ImageListContent> ContentList
        {
            get => _contentList;
            set
            {
                _contentList = value;
                OnPropertyChanged();
            }
        }

        private ReadOnlyObservableCollection<ImageUserCollection> userCollection;
        public ReadOnlyObservableCollection<ImageUserCollection> UserCollection
        { 
            get => userCollection; 
            set 
            {
                userCollection = value;
                OnPropertyChanged(); 
            } 
        }

        private HostControllerBase? _navigateHost=null;
        private IOwnerSetter? OwnerSetter;
        private ImageProcessor processor = new ImageProcessor();
        private long ContentLoadingCount = 0;
        private CancellationTokenSource CancelControl = new CancellationTokenSource();
        private PageState pageState = PageState.Home;
        private bool needUpdate = true;
        private readonly myDB db = new myDB();


        private CommandCarrier _ListViewItemInputDown;
        public CommandCarrier ListViewItemInputDown
        {
            get { return _ListViewItemInputDown; }
            set { _ListViewItemInputDown = value; OnPropertyChanged(); }
        }

        public CommandCarrier<string> SearchButtonDown { get; set; }
        public CommandCarrier ScrollToEnd { get; set; }
        public CommandCarrier<SelectedTagObject> ListViewTagClick { get; set; }
        public CommandCarrier<CheckBoxModalEventArgs> OnCollectionChanged { get; set; }

        public CommandCarrier<ImageListContent> OpenContentSetting { get; set; }


        private int selectedIndex=0;
        public int SelectedIndex { get=>selectedIndex; set { selectedIndex = value; OnPropertyChanged(); } }

        private string searchText;
        public string SearchText { get => searchText; set { searchText = value; OnPropertyChanged(); } }
        public bool IsInputText { set { InputTextChange.Execute(value); } }
        public CommandCarrier<bool> InputTextChange;

        public MainPageController()
        {
            ListViewItemInputDown = new CommandCarrier(() =>
                {
                    needUpdate = false;
                    var iter = ContentList.GetEnumerator(ScrollToEnd, SelectedIndex);
                    _navigateHost?.Move<ImageView>(ControllerStore.Get<ImageViewController>().Init(iter, _navigateHost, processor.UI_dispatcher));
                }
             );
            ScrollToEnd = new CommandCarrier(() =>
            {
                if(Interlocked.Read(ref ContentLoadingCount) == 0)
                {
                    List<DBContent> list = GetLatestContent((ulong)ContentList.Count, (ulong)ContentList.Count + 10);
                    AddContentList(list, CancelControl.Token);
                }
            });
            SearchButtonDown = new CommandCarrier<string>((s) =>
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    pageState = PageState.Search;
                    _=Search(s);
                    needUpdate = true;
                }
                else
                {
                    UpdateToHome();
                }
            });
            ListViewTagClick = new CommandCarrier<SelectedTagObject>((o)=>
            {
                if (o is null) return;
                StringBuilder sb = new StringBuilder().Append('\"').Append(SearchDBHelper.FieldDictionary[o.filed]).Append(':').Append(o.tag).Append('\"');
                SearchText = sb.ToString();
                SearchButtonDown.Execute(SearchText);
            });
            UserCollection = Config.UserCollection.Collection;
            OnCollectionChanged = new CommandCarrier<CheckBoxModalEventArgs>((collection) =>
            {
                //TODO : add collection
            });
            OpenContentSetting = new CommandCarrier<ImageListContent>((value) =>
            {
                if (value is null) return;
                App.OpenContentSettingWindow(value, OwnerSetter);
            });
        }

        public MainPageController Init(HostControllerBase? navigateHost, CommandCarrier<bool> isInputTextChange, IOwnerSetter? setter=null)
        {
            if (navigateHost != null) _navigateHost = navigateHost;
            if (isInputTextChange != null) InputTextChange = isInputTextChange;
            OwnerSetter = setter;
            if (needUpdate)
            {
                List<DBContent> _contentsList = new List<DBContent>(0);
                WorkSpaceScanner scanner = new WorkSpaceScanner(db);
                try
                {
                    while (needUpdate)
                    {
                        _contentsList = GetLatestContent(0, 10);
                        if (!(scanner.ValidationContentWithFix(_contentsList, out needUpdate) || needUpdate))
                        {
                            FileUtil.Log($"Critical Error Occurred \n ValidationContent Error \n system log NeedNewContents : {needUpdate}");
                            App.OpenModal($"Critical Error Occurred\n please check the log");
                            _navigateHost?.Close(true);
                            return this;
                        }
                    }
                }
                finally
                {
                    scanner.DisposeWithCloseDBLink();
                }
                SetDBList(_contentsList);
                needUpdate = false;
            }
            return this;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            if (needUpdate)
            {
                UpdateToHome();
            }
        }

        public void checkUpdateNeeds()
        {
            if(pageState == PageState.Search)
            {
                needUpdate = true;
            }
        }

        public void UpdateToHome()
        {
            SearchText = string.Empty;
            var result = GetLatestContent(0, 10);
            SetDBList(result);
            pageState = PageState.Home;
            needUpdate = false;
        }

        private async Task Search(string data)
        {
            var list = await Task.Factory.StartNew((value) =>
            {
                if (value is string svalue)
                    return SearchEngine.Search(svalue);
                else
                    return new List<long>(0);
            },data,default,TaskCreationOptions.DenyChildAttach,TaskScheduler.Default);
            var result = db.Execute((connection) =>
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                var table = DBHelper.GetTable<Table.ContentTable>();
                StringBuilder sb = new StringBuilder("select * from ").Append(table.TableName).Append(" where ").Append(table.ContentID).Append(" in (");
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append("?");
                    command.Parameters.Add(new SQLiteParameter() { Value = list[i] });
                    if (i < list.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append(')');
                command.CommandText = sb.ToString();
                var reader = command.ExecuteReader();
                return db.ParseContent(reader);
            });
            SetDBList(result);
        }

        /// <summary>
        /// CancellationTokenSource is cancel and set new CancellationTokenSource
        /// </summary>
        private void SetCancelControl()
        {
            if (CancelControl != null)
            {
                CancelControl.Cancel();
                CancelControl.Dispose();
            }
            CancelControl = new CancellationTokenSource();
        }

        private void AddContentList(List<DBContent> DBcontents, CancellationToken token)
            => AddContentList(DBcontents,0,DBcontents.Count, token);

        private void AddContentList(List<DBContent> DBcontents, int startIndex, int endIndex, CancellationToken token)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                ImageListContent content = new ImageListContent(DBcontents[i],false);
                content.OnRightClick = OpenContentSetting;
                Interlocked.Increment(ref ContentLoadingCount);
                ContentList.Add(content);
                Task.Factory.StartNew((value) =>
                {
                    if (value is ThreadDataObject data)
                    {
                        _=SetContent((ImageListContent)data.value, data.token);
                    }
                }, new ThreadDataObject() { value = content, token = token }, token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            }
        }

        private async Task SetContent(ImageListContent content, CancellationToken token)
        {
            var image = await processor.ReadImageData(FileUtil.GetTitleFilePath(content.Path), token, processor.UI_dispatcher,
                (MagickReadSettings setting) =>
                {
                    setting.Height = 200;
                    setting.Width = 200;
                    setting.AntiAlias = false;
                }, (ImageData image) =>
                {
                    image[0].Thumbnail(new MagickGeometry(200, 200) { IgnoreAspectRatio = false });
                });
            if(image is not null)
                content.Source = await processor.ToBitmapSource(image[0], processor.UI_dispatcher, token);
            Interlocked.Decrement(ref ContentLoadingCount);
        }

        private void SetDBList(List<DBContent> list)
            => SetDBList(list, 0, list.Count);

        private void SetDBList(List<DBContent> list, int startIndex, int endIndex)
        {
            SetCancelControl();
            ContentList.Clear();
            AddContentList(list, startIndex, endIndex, CancelControl.Token);
        }

        /// <summary>
        /// GetLatestContent
        /// </summary>
        /// <param name="offset">skip content count</param>
        /// <param name="count">get content count</param>
        /// <returns></returns>
        public List<DBContent> GetLatestContent(ulong offset, ulong count)
        {
            return db.Execute((connection) =>
            {
                using SQLiteCommand command = new SQLiteCommand(connection);
                var table = DBHelper.GetTable<Table.ContentTable>();
                command.CommandText = $"select * from {table.TableName} order by {table.ModifyTime} DESC limit @{nameof(count)} offset @{nameof(offset)}";
                command.Parameters.AddWithValue($"@{nameof(count)}", count);
                command.Parameters.AddWithValue($"@{nameof(offset)}", offset);
                using SQLiteDataReader reader = command.ExecuteReader();
                return db.ParseContent(reader);
            });
        }

    }
}
