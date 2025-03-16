using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using System.Windows;
using WPF_Practice.controller;
using WPF_Practice.view;

namespace WPF_Practice.model
{
    public static class Config
    {
        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private static ConfigObject _configObject;

        /// <summary>
        /// Changing this Getter does not change the setting
        /// </summary>
        public static ConfigObject ConfigObject
        {
            get
            {
                _lock.EnterReadLock();
                var s = new ConfigObject(_configObject);
                _lock.ExitReadLock();
                return s;
            }
        }
        public static List<string> SetWorkSpace
        {
            set
            {
                _lock.EnterWriteLock();
                _configObject.WorkSpace.AddRange(value);
                _lock.ExitWriteLock();
            }
        }
        static Config()
        {
            _configObject= new ConfigObject();
        }
        /// <summary>
        /// read user config
        /// </summary>
        /// <returns>
        /// <see langword="true"/> means writing the ConfigObject was successful. <br/>
        /// <see langword="false"/> requires ConfigObject init.
        /// </returns>
        public static bool readUserConfig()
        {
            using (myDB db = new myDB())
            {
                return db.Execute((connection) =>
                {
                    using SQLiteCommand command = new SQLiteCommand(connection);
                    command.CommandText = $"select * from {DBHelper.ConfigWorkSpaceTableName}";
                    using SQLiteDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return false;
                    }
                    ConfigObject config = ConfigObject;
                    while (reader.Read())
                    {
                        config.WorkSpace.Add(reader.GetString(0));
                    }
                    ChangeConfig(config);
                    return true;
                });
            }
        }

        public static void OpenUserConfigWindow(Window? owner=null, bool Cancelable=true)
        {
            
            UserSettingController? controller = ControllerStore.Get<UserSettingController>();
            UserSettingWindow window = new UserSettingWindow();
            controller.Cancelable = Cancelable;
            window.DataContext = controller;
            if (owner != null)
            {
                window.Owner = owner;
            }
            else
            {
                window.Owner = Application.Current.MainWindow;
            }
            window.ShowInTaskbar = false;
            bool? result=window.ShowDialog();
            if(result==true)
            {
                ControllerStore.Set(controller);
                controller.SetOption(ConfigObject);
            }
        }

        /// <summary>
        /// chnage config.
        /// not change config in db
        /// </summary>
        /// <param name="value"></param>
        public static void ChangeConfig(ConfigObject value)
        {
            _lock.EnterWriteLock();
            _configObject = value;
            _lock.ExitWriteLock();
        }

        /// <summary>
        /// config change in db and local
        /// </summary>
        /// <param name="NewConfig">config to be changed</param>
        /// <returns>number of rows inserted/updated<br/>
        /// -1 is Failure due to exception
        /// </returns>
        public static int AcceptConfig(ConfigObject NewConfig)
        {
            _lock.EnterWriteLock();
            try
            {
                int _resultRows = new myDB().Execute((connection) =>
                {
                    int _result = 0;
                    using SQLiteCommand command = new SQLiteCommand($"insert into {DBHelper.ConfigWorkSpaceTableName} values(@{nameof(NewConfig.WorkSpace)})", connection);
                    for (int i = 0; i < NewConfig.WorkSpace.Count; i++)
                    {
                        command.Parameters.AddWithValue(nameof(NewConfig.WorkSpace), NewConfig.WorkSpace[i]);
                        _result += command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                    return _result;
                });
                _configObject = NewConfig;
                return _resultRows;
            }
            catch
            {
                App.OpenModal("DB Error \n Restore to previous settings");
                return -1;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
