using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Practice.model
{
    public class DBHelper
    {
        public enum Table
        {
            Content,
            Aritst,
            Collection,
        }
        public static string getTableName(Table table)
        {
            return table switch
            {
                Table.Content => ContentTableName,
                Table.Aritst => ArtistTagTableName,
                Table.Collection=>CollectionTagTableName,
                _ => throw new ArgumentException("Invalid Argument Value"),
            };
        }
        public static string getTaggingTableName(Table table)
        {
            return table switch
            {
                Table.Aritst => ArtistTaggingTableName,
                Table.Collection => CollectionTaggingTableName,
                _ => throw new ArgumentException("Invalid Argument Value"),
            };
        }
        internal const string ContentTableName = "ContentTable";
        internal const string ContentID = "ContentID";
        internal const string ContentTable_ContentName = "Name";
        internal const string ContentTable_ModifyTime = "ModifyTime";
        internal const string ArtistTagTableName = "ArtistTable";
        internal const string ArtistTaggingTableName = "ArtistTaggingTable";
        internal const string AllTagTableNameColumn = "TagName";
        internal const string CollectionTagTableName = "CollectionTable";
        internal const string CollectionTaggingTableName = "CollectionTaggingTable";
        internal const string ConfigTableName = "Config";
        internal const string ConfigTableTimeName = "DBSavedTime";
        internal const string SeriesTableName = "SeriesTable";
        internal const string SeriesTaggingTableName = "SeriesTaggingTableName";
        internal const string ConfigWorkSpaceTableName = "ConfigWorkSpace";


    }
}
