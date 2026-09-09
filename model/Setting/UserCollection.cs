using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Threading;
using Tviewer.model.DB;

namespace Tviewer.model.Setting
{
    public static partial class Config
    {
        public static class UserCollection
        {
            private static ObservableCollection<ImageUserCollection> collection = null;
            private static ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
            public static ReadOnlyObservableCollection<ImageUserCollection> Collection
            {
                get
                {
                    try
                    {
                        lockSlim.EnterUpgradeableReadLock();
                        if (collection is null)
                        {
                            GetCollection();
                        }
                    }
                    finally
                    {
                        lockSlim.ExitUpgradeableReadLock();
                    }
                    return new ReadOnlyObservableCollection<ImageUserCollection>(collection);
                }
            }

            public static void Add(ImageUserCollection item)
            {
                try
                {
                    lockSlim.EnterWriteLock();
                    collection.Add(item);
                }
                finally
                {
                    lockSlim.ExitWriteLock();
                }
            }

            public static void Remove(int index)
            {
                try
                {
                    lockSlim.EnterWriteLock();
                    collection.RemoveAt(index);
                }
                finally
                {
                    lockSlim.ExitWriteLock();
                }
            }

            public static void Reset(int index)
            {
                GetCollection();
            }

            private static void GetCollection()
            {
                try
                {
                    using myDB db = new myDB();
                    lockSlim.EnterWriteLock();
                    var list = db.Execute((connection) =>
                    {
                        using SQLiteCommand command = new SQLiteCommand(connection);
                        var table = DBHelper.GetTable<Table.CollectionTable>();
                        command.CommandText = $"select {table.TagID}, {table.TagName} from {table.TableName};";
                        using SQLiteDataReader reader = command.ExecuteReader();
                        List<ImageUserCollection> temp = new List<ImageUserCollection>();
                        if (!reader.HasRows)
                        {
                            return temp;
                        }
                        while (reader.Read())
                        {
                            temp.Add(new ImageUserCollection() { CollectionID = reader.GetInt64(0), Name = reader.GetString(1) });
                        }
                        return temp;
                    });
                    if (collection is not null)
                    {
                        collection.Clear();
                        foreach (var item in list)
                        {
                            collection.Add(item);
                        }
                    }
                    else
                    {
                        collection = new ObservableCollection<ImageUserCollection>(list);
                    }
                }
                finally
                {
                    lockSlim.ExitWriteLock();
                }
            }
        }
    }
}
    
