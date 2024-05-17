namespace WGUAppV2;
using Plugin.LocalNotification;

public partial class CourseView : ContentPage
{
    public Course Course { get; set; }

    public CourseView(Course course)
    {
        InitializeComponent();
        Course = course;
        BindingContext = course;
    }

    private async void OnShareNotesClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Course.Notes))
        {
            await DisplayAlert("No Content", "There are no notes to share.", "OK");
            return;
        }

        await Share.RequestAsync(new ShareTextRequest
        {
            Title = "Share Notes",
            Text = Course.Notes,
            Subject = "Notes for " + Course.CourseName
        });
    }

    private void NotifyCourseDatesClicked(object sender, EventArgs e)
    {
        var courseStartDate = Course.Start.ToString("MM/dd/yyyy");
        var courseEndDate = Course.End.ToString("MM/dd/yyyy");

        var notification = new NotificationRequest
        {
            NotificationId = new Random().Next(), // Ensure unique ID
            Title = $"Reminder for {Course.CourseName}",
            Description = $"Course starts on {courseStartDate} and ends on {courseEndDate}",
            ReturningData = "Detailed data here", // Optional data that is returned to the app when tapped
            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
            {
                ChannelId = "course_notifications"
            }
        };

        LocalNotificationCenter.Current.Show(notification);
    }

}