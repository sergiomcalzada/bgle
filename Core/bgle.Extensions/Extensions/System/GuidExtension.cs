namespace System
{
    public static class GuidExtension
    {
        public static string ToUserString(this Guid guid)
        {
            return guid.ToString().Replace("-", "");
        }
    }
}