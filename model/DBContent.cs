using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace WPF_Practice.model
{
    public class DBContent
    {
        /// <summary>
        /// -1 is not exist
        /// </summary>
        public long ID { get; set; }
        public string Name { get; set; }
        public string Path {  get; set; }
        public long Rating { get; set; }
        public long CreateTime { get; set; }
        public long ModifyTime { get; set; }
        public long SeriesOrder { get; set; }
        public List<string> Artist { get; set; }
        public List<string> Group { get; set; }
        public List<string> Series { get; set; }
        public List<string> Other { get; set; }
        public List<string> Collection { get; set; }

        public DBContent(DBContent value)
        {
            ID= value.ID;
            Name= value.Name;
            Path = value.Path;
            Rating = value.Rating;
            CreateTime = value.CreateTime;
            ModifyTime = value.ModifyTime;
            SeriesOrder = 1;
            Artist = new List<string>(value.Artist);
            Group = new List<string>(value.Group);
            Series = new List<string>(value.Series);
            Other = new List<string>(value.Other);
            Collection = new List<string>(value.Collection);
        }

        public DBContent(string path,long id = -1, string name="", long rating = 0,long createTime = 0, long modifyTime = 0,long seriesOrder = 1, List<string>? artist = null, List<string>? group=null, List<string>? series = null, List<string>? other= null, List<string>? collection = null)
        {
            ID = id;
            Name = string.IsNullOrEmpty(name) ? path : name;
            Path = path;
            Rating = rating;
            CreateTime = createTime;
            ModifyTime = modifyTime;
            SeriesOrder = seriesOrder;
            Artist = artist == null ? new List<string>() : artist ;
            Group = group == null ? new List<string>() : group;
            Series = series == null ? new List<string>() : series;
            Other = other == null ? new List<string>() : other;
            Collection = collection == null ? new List<string>() : collection;
        }

        public static bool ContainList(IList<string> list, string tag)
        {
            return list.Contains(tag, new DBContentTagComparer(StringComparison.CurrentCultureIgnoreCase));
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(Name)).Append(" : ").Append(Name).Append("\t");
            AppendIList(sb, Artist);
            AppendIList(sb, Group);
            sb.Append(nameof(ModifyTime)).Append(" : ").Append(ModifyTime).Append("\t");
            sb.Append(nameof(Path)).Append(" : ").Append(Path).Append("\t");
            return sb.ToString();
        }

        private void AppendIList(StringBuilder stringBuilder, List<string> list, [CallerArgumentExpression(nameof(list))] string name="List")
        {
            stringBuilder.Append(name).Append(" : ");
            if (list.Count == 0)
            {
                stringBuilder.Append("null\t");
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i]).Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2).Append('\t');
        }
    }
}
