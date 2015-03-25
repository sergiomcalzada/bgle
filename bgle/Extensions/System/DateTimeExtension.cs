namespace System
{
    public static class DateTimeExtension
    {
        public static int ToDaySlot(this DateTime date)
        {
            return 10000 * date.Year + 100 * date.Month + date.Day;
        }
    }
}