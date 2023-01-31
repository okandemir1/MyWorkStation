namespace OkanDemir.WebUI.Cms.Helpers
{
    public static class DateExtension
    {
        public static string ToTimeAgo(this DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} yıl önce", years);
            }
            if (span.Days >= 7)
                return String.Format("{0} hft önce", (int)(span.Days / 7));
            if (span.Days < 7 && span.Days > 0)
                return String.Format("{0} gün önce", span.Days);
            if (span.Hours > 0)
                return String.Format("{0} saat önce", span.Hours);
            if (span.Minutes > 0)
                return String.Format("{0} dk önce", span.Minutes);
            if (span.Seconds > 5)
                return String.Format("{0} sn önce", span.Seconds);
            if (span.Seconds <= 5)
                return "0s";
            return string.Empty;
        }

        public static DateTime ToUnixTimeToDateTime(this string timestamp)
        {
            long convertThis = long.Parse(timestamp);

            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds((double)convertThis);
            dateTime = dateTime.ToLocalTime();  // Change GMT time to your timezone
            return dateTime;
        }
    }
}
