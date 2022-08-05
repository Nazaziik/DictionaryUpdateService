namespace DictionaryService
{
    public class AppConfig
    {
        public static string FtpIp { get; set; }

        public static string FtpUsername { get; set; }

        public static string FtpPassword { get; set; }

        public static string DbConString { get; set; }

        public static int UpdateHour { get; set; }

        public AppConfig() { }
    }
}
