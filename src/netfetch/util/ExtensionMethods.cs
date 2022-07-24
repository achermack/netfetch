namespace netfetch.util
{
    public static class ExtensionMethods
    {
        public static string ToHumanTimeString(this TimeSpan span)
        {
            string[] timeSpanStrings = { span.Days > 0 ? $"{span.Days} days" : "", span.Hours > 0 ? $"{span.Hours} hours" : "", span.Minutes > 0 ? $"{span.Minutes} minutes" : "", span.Seconds > 0 ? $"{span.Seconds} seconds" : "" };
            return string.Join(", ", timeSpanStrings.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

    }
}