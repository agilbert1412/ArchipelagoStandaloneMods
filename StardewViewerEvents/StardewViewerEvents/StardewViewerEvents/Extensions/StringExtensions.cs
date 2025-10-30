using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewViewerEvents.Extensions
{
    public static class StringExtensions
    { 
        public static string SanitizeEntityName(this string entityName)
        {
            return entityName == null ? "" : entityName.Replace(" ", "").ToLower();
        }
    }
}
