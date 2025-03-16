using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;
using WPF_Practice.Interfaces;
using WPF_Practice.model;

namespace WPF_Practice
{
    //SQLite 구현은 100% 동기식. thread safe하
    //https://stackoverflow.com/a/34478670/21337092
    public class myDB : IDisposable
    {
        private const string _dataBaseName = "data.db";
        readonly string path = $"{Environment.CurrentDirectory}\\{_dataBaseName}";
        //connection pooling connection
        private readonly static string datasource = $"Data Source={Environment.CurrentDirectory}\\DB\\{_dataBaseName};Pooling=True;Max Pool Size=50; pragma foreign_keys = ON";

        private readonly SQLiteConnection connection = new SQLiteConnection(datasource);
        private bool disposedFlag=false;


        public bool InsertDBContent(DBContent content)
        {
            return Execute((connection) =>
            {
                string sqltext = $"INSERT INTO {DBHelper.ContentTableName} VALUES(@{nameof(content.ID)},@{nameof(content.Name)},@{nameof(content.Path)},@{nameof(content.Rating)},@{nameof(content.CreateTime)},@{nameof(content.ModifyTime)});";
                using SQLiteCommand command = new SQLiteCommand(sqltext, connection);
                command.Parameters.AddWithValue($"@{nameof(content.ID)}", content.ID < 0 ? null : content.ID);
                command.Parameters.AddWithValue($"@{nameof(content.Name)}", content.Name);
                command.Parameters.AddWithValue($"@{nameof(content.Rating)}", content.Rating);
                command.Parameters.AddWithValue($"@{nameof(content.Path)}", content.Path);
                command.Parameters.AddWithValue($"@{nameof(content.CreateTime)}", content.CreateTime);
                command.Parameters.AddWithValue($"@{nameof(content.ModifyTime)}", content.ModifyTime);
                if (command.ExecuteNonQuery() == 0)
                {
                    return false;
                }
                AutoInsertTags(content.Artist, DBHelper.Table.Aritst);
                AutoInsertTags(content.Collection, DBHelper.Table.Collection);
                return true;
            });
        }

        /// <summary>
        /// get tag name column name in tag table 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetTagNameColumnName(SQLiteConnection connection, DBHelper.Table table, int index)
        {
            SQLiteCommand command = new SQLiteCommand($" PRAGMA table_info('{DBHelper.getTableName(table)}');", connection);
            using (var reader = command.ExecuteReader())
            {
                for (int i = 0; i <= index; i++)
                {
                    reader.Read();
                }
                return reader.GetString(1);
            }
        }

        /// <summary>
        /// check exists artist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="content">tag list in content</param>
        /// <param name="tableName">tag table name</param>
        /// <returns>
        ///  return <see langword="true"/> if tag list of <paramref name="content"/> found in <paramref name="tableName"/>.<br/>
        ///  also, always Returns <see langword="true"/> when the <paramref name="content"/> is null or count 0.<br/>
        ///  return <see langword="false"/>. if DB query result is not equal to <paramref name="content"/> count  or less than 1.
        ///  <exception cref="ArgumentException">sdfs</exception>
        /// </returns>
        private bool ExistsTags<T>(List<T>? content, DBHelper.Table table)
        {
            if (content == null || content.Count==0)
            {
                return true;
            }
            SQLiteCommand command = new SQLiteCommand(connection);
            string _colName=GetTagNameColumnName(connection,table,1);
            StringBuilder sb = new StringBuilder($"select count(*) from {DBHelper.getTableName(table)} where ");
            for (int i = 0; i < content.Count; i++)
            {
                sb.Append($"{_colName} like ?");
                var sqlParam=new SQLiteParameter();
                sqlParam.Value=content[i];
                command.Parameters.Add(sqlParam);
                if (i < content.Count - 1)
                {
                    sb.Append(" or ");
                }
            }
            sb.Append(';');
            command.CommandText = sb.ToString();
            long result = (long)command.ExecuteScalar();
            return result > 0 && result == content.Count;
        }


        /// <summary>
        /// Auto Insert tags
        /// </summary>
        /// <typeparam name="T">list type in content</typeparam>
        /// <param name="command"></param>
        /// <param name="values">list in content</param>
        /// <param name="table"></param>
        /// <returns><see langword="true"/>, if insert success</returns>
        private bool AutoInsertTags<T>(List<T>? values, DBHelper.Table table)
        {
            if (ExistsTags(values, table))
            {
                return false;
            }

#pragma warning disable CS8602 // values null check already check in ExistsTags
            List<int> insertArtistIndex = new List<int>(values.Count);
#pragma warning restore CS8602
            for (int i = 0; i < values.Count; i++)
            {
                insertArtistIndex.Add(i);
            }
            string _colName=GetTagNameColumnName(connection, table,1);
            SQLiteCommand command = new SQLiteCommand(connection);
            //select
            StringBuilder sb = new StringBuilder($"select {_colName} from {DBHelper.getTableName(table)} where ");
            for (int i = 0; i < values.Count; i++)
            {
                sb.Append($"{_colName} like ?");
                var sqlParameter=new SQLiteParameter();
                sqlParameter.Value = values[i];
                command.Parameters.Add(sqlParameter);
                if (i < values.Count - 1)
                {
                    sb.Append(" or ");
                }
            }
            sb.Append(';');
            command.CommandText = sb.ToString();
            //Debug.WriteLine(command.CommandText);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                int _deleteIndex=-1;
                while (reader.Read())
                {
                    if (reader.GetDataTypeName(0).Equals("TEXT"))
                    {
                        string _target = reader.GetString(0);
                        _deleteIndex = values.FindIndex((s) =>{return string.Equals(_target, s?.ToString(), StringComparison.CurrentCultureIgnoreCase);});
                        Debug.WriteLine($"{_target} {_deleteIndex}");
                    }
                    else if(reader.GetDataTypeName(0).Equals("INTEGER"))
                    {
                        long _target = reader.GetInt64(0);
                        _deleteIndex = values.FindIndex((s)=> { return _target.Equals(s); });
                        Debug.WriteLine($"{_target} {_deleteIndex}");
                    }
                    if (_deleteIndex != -1)
                    {
                        insertArtistIndex.Remove(_deleteIndex);
                    }
                }
            }

            //insert
            if (insertArtistIndex.Count > 0)
            {
                command.Parameters.Clear();
                sb.Clear();
                sb.Append($"insert into {DBHelper.getTableName(table)} values");
                for (int i = 0; i < insertArtistIndex.Count; i++)
                {
                    sb.Append(" (null,?)");
                    var parameter = new SQLiteParameter();
                    parameter.Value = values[insertArtistIndex[i]];
                    command.Parameters.Add(parameter);
                    if (i < insertArtistIndex.Count - 1)
                    {
                        sb.Append(',');
                    }
                    else
                    {
                        sb.Append(';');
                    }
                }
                command.CommandText = sb.ToString();
                command.ExecuteNonQuery();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentName"></param>
        /// <returns>
        /// if the content Exists, return  ContentID<br/>
        /// if it does not Exists, return -1
        /// </returns>
        public int GetContentID(string contentName)
        {
            return Execute((connection) =>
            {
                using SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = $"select {DBHelper.ContentID} from {DBHelper.ContentTableName} where {DBHelper.ContentTable_ContentName} like @paraName";
                command.Parameters.AddWithValue("@paraName", contentName);
                using SQLiteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                return -1;
            });
        }

        /// <summary>
        /// </summary>
        /// <returns>if the value is not found, 0 return </returns>
        public long LoadDBSavedTicks()
        {
            return Execute((connection) =>
            {
                using SQLiteCommand command = new SQLiteCommand($"select {DBHelper.ConfigTableTimeName} from {DBHelper.ConfigTableName}", connection);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return reader.GetInt64(0);
                }
                return 0;
            });
        }

        public bool SaveDBSavedTicksNow()
        {
            return Execute((connection) =>
            {
                long nowTimeTick = DateTime.UtcNow.Ticks;
                using SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = $"update {DBHelper.ConfigTableName} set {DBHelper.ConfigTableTimeName} = @{nameof(nowTimeTick)}";
                command.Parameters.AddWithValue($"@{nameof(nowTimeTick)}", nowTimeTick);
                return command.ExecuteNonQuery() != 0;
            });
        }

        public void Optimizing(SQLiteConnection connection)
        {
            using SQLiteCommand command = new SQLiteCommand("PRAGMA optimize;", connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// How to use<br/>
        /// Execute((connection)=>{<br/>
        /// using SQLiteCommand command = new SQLiteCommand(connection);<br/>
        /// ....<br/>
        /// });
        /// </summary>
        /// <typeparam name="T">input function result type</typeparam>
        /// <param name="DB_Delegate">input function</param>
        /// <returns></returns>
        public T Execute<T>(DBDelegate<T> DB_Delegate)
        {
            try
            {
                connection.Open();
                T result;
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    result = DB_Delegate(connection);
                    transaction.Commit();
                }
                return result;
            }
            finally
            { 
                connection.Close();
            }
        }

        public bool TryExecute<T>(DBDelegateNullable<T> DB_Delegate,out T? result)
        {
            try
            {
                bool resultFlag;
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    resultFlag = DB_Delegate(connection,out result);
                    transaction.Commit();
                }
                return resultFlag;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// How to use<br/>
        /// Execute((connection)=>{<br/>
        /// using SQLiteCommand command = new SQLiteCommand(connection);<br/>
        /// ....<br/>
        /// });
        /// </summary>
        /// <param name="DB_Delegate">input function</param>
        public void Execute(DBDelegate DB_Delegate)
        {
            try
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    DB_Delegate(connection);
                    transaction.Commit();
                }
            }
            finally
            { 
                connection.Close();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedFlag)
            {
                if (disposing)
                {
                    
                }
                connection.Dispose();
                disposedFlag = true;
            }
        }

        ~myDB()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
