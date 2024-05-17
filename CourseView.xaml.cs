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
            NotificationId = new Random().Next(), // Ensures unique ID
            Title = $"Reminder for {Course.CourseName}",
            Description = $"Course starts on {courseStartDate} and ends on {courseEndDate}",
            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
            {
                ChannelId = "course_notifications"
            }
        };

        LocalNotificationCenter.Current.Show(notification);
    }

    private void NotifyAssessmentDatesClicked(object sender, EventArgs e)
    {
        // Format the start and end dates for both assessments
        var oaStartDate = Course.OaStart.ToString("MM/dd/yyyy");
        var oaEndDate = Course.OaEnd.ToString("MM/dd/yyyy");
        var paStartDate = Course.PaStart.ToString("MM/dd/yyyy");
        var paEndDate = Course.PaEnd.ToString("MM/dd/yyyy");

        // Create the notification content
        var notification = new NotificationRequest
        {
            NotificationId = new Random().Next(), // Ensures unique ID
            Title = "Assessment Dates",
            Description = $"Objective Assessment: {oaStartDate} to {oaEndDate}\n" +
                          $"Performance Assessment: {paStartDate} to {paEndDate}",
            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
            {
                ChannelId = "course_notifications" 
            }
        };

        // Show the notification
        LocalNotificationCenter.Current.Show(notification);
    }

}