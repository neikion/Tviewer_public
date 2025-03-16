using ImageMagick;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF_Practice.Interfaces;
using WPF_Practice.model;
using WPF_Practice.view;

namespace WPF_Practice.controller
{
    class MainPageController : ControllerBase
    {
        /// <summary>
        /// option category list
        /// </summary>
        public ObservableCollection<ImageListContent> ContentList { get; set; }
        public ObservableCollection<ImageUserCollection> UserCollection { get; set; }
        private List<DBContent> _DBContentList = new List<DBContent>();
        protected Dispatcher UI_Dispatcher;

        private CommandCarrier<object[]> _ListViewItemInputDown;
        public CommandCarrier<object[]> ListViewItemInputDown
        {
            get { return _ListViewItemInputDown; }
            set { _ListViewItemInputDown = value; }
        }

        private CommandCarrier _menuButton;
        public CommandCarrier MenuButton
        {
            get { return _menuButton; }
            set { _menuButton = value; OnPropertyChanged(); }
        }
        private CommandCarrier _configButton;
        public CommandCarrier ConfigCommand
        {
            get => _configButton;
            set
            {
                _configButton = value;
                OnPropertyChanged();
            }
        }

        private INavigateHost? _navigateHost=null;

        public MainPageController()
        {
            using myDB db = new myDB();
            ContentList = new ObservableCollection<ImageListContent>();
            UI_Dispatcher = Application.Current.Dispatcher;
            _DBContentList = GetLatestContent(db, 0, 10);
            SetContentList(_DBContentList).ConfigureAwait(false);
            UserCollection = getUserCollectionName();
            _ListViewItemInputDown = new CommandCarrier<object[]>(
                (values) =>
                {
                    if (values == null) return;
                    if (values[1] is System.Windows.Input.KeyEventArgs key)
                    {
                        if (WpfExtensions.RealKey(key) != System.Windows.Input.Key.Enter) return;
                }
                    else if (values[1] is System.Windows.Input.MouseButtonEventArgs mouse)
                {
                        if(mouse.LeftButton!=System.Windows.Input.MouseButtonState.Pressed) return;
                    }
                    else
                    {
                        return;
                    }
                    ImageListContent? item = values[0] as ImageListContent;
                    if (item == null) return;
                    _navigateHost?.Move<ImageView>(new ImageViewController(item.Path,_navigateHost));
                }
             );
            _configButton = new CommandCarrier((o) =>
            {
                Config.OpenUserConfigWindow();
            });

        }
        public MainPageController(INavigateHost navigateHost) : this()
        {
            _navigateHost = navigateHost;
        }

        public async Task SetContentList(List<DBContent> DBcontents)
        {
            for (int i = 0; i < DBcontents.Count; i++)
            {
                ImageListContent content = new ImageListContent(DBcontents[i]);
                await SetContent(content);
                lock (_DBContentList)
                {
                ContentList.Add(content);
            }
        }
        }
        private async Task SetContent(ImageListContent content)
        {
            BitmapSource? source = await ReadImageAsync(FileUtil.GetTitleFilePath(content.Path), default, (ref MagickImage image) =>
            {
                image.Thumbnail(new MagickGeometry(200, 200) { IgnoreAspectRatio = false });
            });
            if (source != null)
            {
                content.Source = source;
            }
        }

        protected async Task<BitmapSource?> ReadImageAsync(string path, CancellationToken token = default, ImageProcess? optionCall = null)
        {
            MagickImage image;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous))
            {

                using (image = new MagickImage())
                {
                    MagickReadSettings settings = new MagickReadSettings();
                    settings.AntiAlias = false;
                    image.Quality = 100;
                    await image.ReadAsync(fs, settings, token).ConfigureAwait(false);
                    optionCall?.Invoke(ref image);

                    try
                    {
                        return await UI_Dispatcher.InvokeAsync<BitmapSource?>(() =>
                        {
                            var result = IMagickImageExtentions.ToBitmapSource(image);
                            result.Freeze();
                            return result;
                        }, DispatcherPriority.Normal, token).Task.ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        Debug.WriteLine($"cancel path {path}");
                        return null;
                    }
                }
            }
        }

        protected ObservableCollection<ImageUserCollection> getUserCollectionName()
        {
            var list = GetCollectionNames();
            ObservableCollection<ImageUserCollection> result=new ObservableCollection<ImageUserCollection>();
            if (list == null)
            {
                return result;
            }
            for (int i = 0; i < list.Count; i++)
            {
                ImageUserCollection collection = new ImageUserCollection();
                collection.Name = list[i];
                result.Add(collection);
            }
            return result;
        }
        public List<string>? GetCollectionNames()
        {
            using myDB db=new myDB();
            return db.Execute((connection) =>
            {
                List<string>? list;
                using SQLiteCommand command = new SQLiteCommand(connection);
                string colName = db.GetTagNameColumnName(connection, DBHelper.Table.Collection, 1);
                command.CommandText = $"select {colName} from {DBHelper.CollectionTagTableName};";
                using SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return null;
                }
                list = new List<string>();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
                return list;
            });
        }

        public List<DBContent> GetLatestContent(myDB db,ulong offset, ulong count)
        {
            List<DBContent>? result = db.Execute((connection) =>
            {
                List<DBContent> contents = new List<DBContent>();
                using SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = $"select * from {DBHelper.ContentTableName} order by {DBHelper.ContentTable_ModifyTime} DESC limit @{nameof(count)} offset @{nameof(offset)}";
                command.Parameters.AddWithValue($"@{nameof(count)}", count);
                command.Parameters.AddWithValue($"@{nameof(offset)}", offset);
                using SQLiteDataReader reader = command.ExecuteReader();
                DBContent content;
                while (reader.Read())
                {
                    content = new DBContent(reader.GetString(2), reader.GetInt64(0), reader.GetString(1), reader.GetInt64(3), reader.GetInt64(4), reader.GetInt64(5));
                    contents.Add(content);
                }
                return contents;
            });
            if(result== null)
            {
                result = new List<DBContent>();
            }
            return result;
        }

    }
}
