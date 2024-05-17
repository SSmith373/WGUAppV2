using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.LocalNotification;

namespace WGUAppV2
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
#if ANDROID_26
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = "Course Notifications";
                var channelDescription = "Notifications for course start and end dates.";
                var channel = new NotificationChannel("course_notifications", channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
#endif
        }
    }
}



