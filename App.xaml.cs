using Plugin.LocalNotification;

namespace WGUAppV2
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            MainPage = new NavigationPage(mainPage);
        }

    }
}
