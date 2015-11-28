using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public static class StringExtensions
    {
        public static string RemoveSpaces(this string value)
        {
            return value.Replace(" ", "");
        }
    }
}
