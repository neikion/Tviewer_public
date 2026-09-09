using System;
using System.Data.SQLite;
using System.Threading;
using Tviewer.model.DB;

namespace Tviewer.model.Setting
{
    public static partial class Config
    {
        private static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private static ConfigObject _configObject;

        /// <summary>
        /// Changing this Getter does not change the setting
        /// </summary>
        public static ConfigObject GetConfigObject
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return new ConfigObject(_configObject);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
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
        public static bool ReadUserConfig()
        {
            using myDB db = new myDB();
            return db.Execute((connection) =>
            {
                using SQLiteCommand command = new SQLiteCommand(connection);
                return ReadUserConfig(command);
            });
        }

        public static bool ReadUserConfig(SQLiteCommand command)
        {
            command.CommandText = $"select * from {DBHelper.GetTable<Table.ConfigTable>().TableName}";
            using SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return false;
            }
            ConfigObject config = new ConfigObject();
            while (reader.Read())
            {
                config.OptionList.Add(new ConfigOption()
                {
                    ID = reader.GetInt64(0),
                    WorkSpace = reader.GetString(1),
                    SaveTime = reader.GetInt64(2)
                });
            }
            _lock.EnterWriteLock();
            try
            {
                _configObject = config;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            return true;
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
            ConfigObject oldConfig = _configObject;
            try
            {
                using myDB db = new myDB();
                int _resultRows=0;
                if (NewConfig.OptionList.Count > 0)
                {
                    #region Restore the previous ID when the Path is identical
                    for (int i = 0; i < NewConfig.OptionList.Count; i++)
                    {
                        for (int k = 0; k < oldConfig.OptionList.Count; k++)
                        {
                            var oldconfig = oldConfig.OptionList[k];
                            var newconfig = NewConfig.OptionList[i];
                            if(newconfig.WorkSpace.Equals(oldconfig.WorkSpace) && newconfig.ID < 0)
                            {
                                newconfig.ID = oldconfig.ID;
                            }
                        }
                    }
                    #endregion

                    _resultRows = db.Execute((connection) =>
                    {
                        int _result = 0;
                        var table = DBHelper.GetTable<Table.ConfigTable>();
                        using SQLiteCommand command=new SQLiteCommand(connection);

                        command.CommandText = $"delete from {table.TableName}";
                        command.ExecuteNonQuery();
                        command.CommandText = $"insert into {table.TableName} values(@{table.ID}, @{table.WorkSpace},@{table.SaveTime})";
                        command.Parameters.Add(table.ID, System.Data.DbType.Int64);
                        command.Parameters.Add(table.WorkSpace, System.Data.DbType.String);
                        command.Parameters.Add(table.SaveTime, System.Data.DbType.Int64);
                        for (int i = 0; i < NewConfig.OptionList.Count; i++)
                        {
                            if (NewConfig.OptionList[i].ID < 0) command.Parameters[0].Value = DBNull.Value;
                            else command.Parameters[0].Value = NewConfig.OptionList[i].ID;
                            command.Parameters[1].Value = NewConfig.OptionList[i].WorkSpace;
                            command.Parameters[2].Value = NewConfig.OptionList[i].SaveTime;
                            _result += command.ExecuteNonQuery();
                        }
                        return _result;
                    });
                }
                _configObject = NewConfig;
                return _resultRows;
            }
            catch
            {
                App.OpenModal("DB Error \n Restore to previous settings");
                _configObject = oldConfig;
                return -1;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
