using System;
using System.Collections.Generic;

namespace Karambit.Text
{
    public class CaseInsensitiveEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y) {
            return x.ToLower() == y.ToLower();
        }

        public int GetHashCode(string obj) {
            return obj.GetHashCode();
        }
    }
}
