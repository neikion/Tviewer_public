using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Practice.model
{
    public class DBContentTagComparer : IEqualityComparer<string>
    {
        private StringComparison _comparison;
        public DBContentTagComparer()
        {
            _comparison = StringComparison.CurrentCulture;
        }
        public DBContentTagComparer(StringComparison comparison)
        {
            _comparison = comparison;
        }
        public bool Equals(string? x, string? y)
        {
            if (x == null || y == null)
            {
                if (x == null && y == null) return true;
                return false;
            }
            return x.Equals(y, _comparison);
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            return obj.GetHashCode();
        }
    }
}
