using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeViewerEvents.Extensions
{
    public static class StringExtensions
    {
        public static string SanitizeName(this string name)
        {
            if (name == null)
            {
                return "";
            }

            return name
                .Replace(" ", "")
                .Replace(":", "")
                .Replace(";", "")
                .Replace("-", "")
                .Replace("_", "")
                .Replace("|", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace(",", "")
                .Replace(".", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("'", "")
                .Replace("`", "")
                .ToLower();
        }
    }
}
