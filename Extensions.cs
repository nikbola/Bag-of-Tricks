using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BagOfTricks
{
    public static class Extensions
    {
        public static bool CustomStartsWith(this string str, string substring)
        {
            for (int i = 0; i < substring.Length; i++)
            {
                if (str[i] != substring[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
