namespace WebServices
{
    using Xamarin.Essentials;

    public static class AppSettings
    {
        //App Center
        const string defaultAppCenterAndroid = "";
        const string defaultAppCenteriOS = "";

        public static string CurrencyRootEndpoint = "https://api.openweathermap.org/data/2.5/forecast";

        public static string CurrencyKey = "";

        public static string CurrencylayerEndpoint;

        static AppSettings()
        {
        }

        public static string AppCenterAnalyticsAndroid
        {
            get => Preferences.Get(nameof(AppCenterAnalyticsAndroid), defaultAppCenterAndroid);
            set => Preferences.Set(nameof(AppCenterAnalyticsAndroid), value);
        }

        public static string AppCenterAnalyticsIos
        {
            get => Preferences.Get(nameof(AppCenterAnalyticsIos), defaultAppCenteriOS);
            set => Preferences.Set(nameof(AppCenterAnalyticsIos), value);
        }
    }
}
