namespace Api.Extensions
{
    public static class IntExtensions
    {
        public static string ToPluralitySuffix(this int num)
        {
            return num > 1 ? "s" : "";
        }
    }
}