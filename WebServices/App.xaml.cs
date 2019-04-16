namespace WebServices
{
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new CurrencylayerView();
        }

        protected override void OnStart()
        {
            /// Disable AppCenter in UI tests
            #if (IS_UI_TEST == false)

            // Handle when your app starts
            AppCenter.Start($"ios={AppSettings.AppCenterAnalyticsIos};" +
                $"android={AppSettings.AppCenterAnalyticsAndroid}",
                typeof(Analytics), typeof(Crashes));
            #endif
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
