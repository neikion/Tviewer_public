using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using Tviewer.Interfaces;
using Tviewer.model;
using Tviewer.model.DB;
using Tviewer.model.Setting;

namespace Tviewer.controller
{
    public class SlidingPanelController : ControllerBase
    {
        private ReadOnlyObservableCollection<ImageUserCollection> _userCollection { get; set; }
        public ReadOnlyObservableCollection<ImageUserCollection> UserCollection
        {
            get => _userCollection;
            set
            {
                _userCollection = value;
                OnPropertyChanged();
            }
        }
        
        private bool _slideState = true;
        public bool SlideState { get => _slideState; set { _slideState = value; OnPropertyChanged(); } }

        private CommandCarrier? _menuCommand;
        public CommandCarrier? MenuCommand {
            get => _menuCommand;
            set
            {
                _menuCommand = value;
                OnPropertyChanged();
            }
        }

        private CommandCarrier _configCommand;
        public CommandCarrier ConfigCommand
        {
            get => _configCommand;
            set
            {
                _configCommand = value;
                OnPropertyChanged();
            }
        }

        private CommandCarrier _homeCommand;

        public CommandCarrier HomeCommand {
            get => _homeCommand;
            set
            {
                _homeCommand = value;
                OnPropertyChanged();
            }
        }

        private CommandCarrier addNewCollectionCommand;
        public CommandCarrier AddNewCollectionCommand 
        { 
            get => addNewCollectionCommand; 
            set { addNewCollectionCommand = value; OnPropertyChanged(); }  
        }

        private INavigateHost _navigateHost;
        private IOwnerSetter? OwnerSetter;

        public SlidingPanelController()
        {
            UserCollection = Config.UserCollection.Collection;
            ConfigCommand = new CommandCarrier(() =>
            {
                App.OpenUserConfigWindow(OwnerSetter);
            });
            AddNewCollectionCommand = new CommandCarrier(() =>
            {
                string result = App.OpenSimpleInputModal("Input Name",out bool cancel, OwnerSetter);
                if (cancel) return;
                using myDB db = new();
                db.Execute((connection) =>
                {
                    SQLiteCommand command = new SQLiteCommand(connection);
                    command.CommandText = $"insert into {DBHelper.GetTable<Table.CollectionTable>().TableName} values(null,@{nameof(result)}) RETURNING {DBHelper.GetTable<Table.CollectionTable>().TagID}";
                    command.Parameters.AddWithValue($"@{nameof(result)}", result);
                    using var reader=command.ExecuteReader();
                    while (reader.Read())
                    {
                        Config.UserCollection.Add(new ImageUserCollection() { Name = result, CollectionID = reader.GetInt64(0) });
                    }
                });
            });
        }

        public SlidingPanelController init(INavigateHost host, CommandCarrier menu,CommandCarrier home, IOwnerSetter? setter=null)
        {
            _navigateHost = host;
            OwnerSetter = setter;
            MenuCommand = menu;
            HomeCommand = home;
            return this;
        }
    }
}
