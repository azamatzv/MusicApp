namespace N_Tier.Application.QuartzConfigPayment
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return date.StartOfMonth().AddMonths(1).AddDays(-1);
        }

        public static bool IsLastDayOfMonth(this DateTime date)
        {
            return date.Date == date.EndOfMonth().Date;
        }

        public static bool IsWithinLastDaysOfMonth(this DateTime date, int days)
        {
            var endOfMonth = date.EndOfMonth();
            var daysUntilEndOfMonth = (endOfMonth - date).Days;
            return daysUntilEndOfMonth < days;
        }
    }
}
