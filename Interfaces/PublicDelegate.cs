using ImageMagick;
using System.Data.SQLite;
using Tviewer.model.ImageData;

namespace Tviewer.Interfaces
{
    //imageview
    public delegate void ImageProcess(ImageData image);
    public delegate void ImagePreProcess(MagickReadSettings settings);

    //DB
    public delegate T DBDelegate<T>(SQLiteConnection connection);
    public delegate bool DBDelegateNullable<T>(SQLiteConnection connection, out T? result);
    public delegate void DBDelegate(SQLiteConnection connection);
}
