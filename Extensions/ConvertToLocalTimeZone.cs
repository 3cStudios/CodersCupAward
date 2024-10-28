namespace CodersCupAward.Extensions
{
    public static class ConvertToLocalTimeZone
    {
        public static DateTime ConvertToTimeZoneTime(this DateTime dateTime, TimeZoneInfo timeZoneInfo)
        {
            var convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);
            return convertedDateTime;
        }


        

    }
}
