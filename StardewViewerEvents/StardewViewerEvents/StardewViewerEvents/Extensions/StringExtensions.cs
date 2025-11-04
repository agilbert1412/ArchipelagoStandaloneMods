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
