using ImageMagick;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Practice.Interfaces
{
    //imageview
    public delegate void ImageProcess(ref MagickImage image);

    //DB
    public delegate T DBDelegate<T>(SQLiteConnection connection);
    public delegate bool DBDelegateNullable<T>(SQLiteConnection connection, out T? result);
    public delegate void DBDelegate(SQLiteConnection connection);
}
