namespace WGUAppV2;

public partial class TermView : ContentPage
{
    public bool IsAddCourseVisible { get; set; } = false;
    private int _termId; // Store term ID
    private readonly DBService _dbService;
    private int _editCourseId;

    // Properties for binding
    public string TermName { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TermView(int termId, string termName, DateTime start, DateTime end, DBService dbService)
    {
        InitializeComponent();
        _termId = termId;
        TermName = termName;
        Start = start;
        End = end;

        _dbService = dbService;

        BindingContext = this; // Set this after initializing properties

        Task.Run(async () => coursesListView.ItemsSource = await _dbService.GetCoursesByTermId(termId));
    }

    private void ToggleAddCourse(object sender, EventArgs e)
    {
        IsAddCourseVisible = !IsAddCourseVisible;
        OnPropertyChanged(nameof(IsAddCourseVisible));
    }

    private async void SaveCourseClicked(object sender, EventArgs e)
    {
        // Validation for Course Name
        if (string.IsNullOrWhiteSpace(courseNameEntry.Text) ||
            string.IsNullOrWhiteSpace(courseStartDatePicker.Date.ToString()) ||
            string.IsNullOrWhiteSpace(courseEndDatePicker.Date.ToString()) ||
            string.IsNullOrWhiteSpace(courseStatusPicker.SelectedItem?.ToString()) ||
            string.IsNullOrWhiteSpace(instructorNameEntry.Text) ||
            string.IsNullOrWhiteSpace(instructorPhoneEntry.Text) ||
            string.IsNullOrWhiteSpace(instructorEmailEntry.Text) ||
            string.IsNullOrWhiteSpace(oaNameEntry.Text) ||
            string.IsNullOrWhiteSpace(oaStartDatePicker.Date.ToString()) ||
            string.IsNullOrWhiteSpace(oaEndDatePicker.Date.ToString()) ||
            string.IsNullOrWhiteSpace(paNameEntry.Text) ||
            string.IsNullOrWhiteSpace(paStartDatePicker.Date.ToString()) ||
            string.IsNullOrWhiteSpace(paEndDatePicker.Date.ToString()))
        {
            await DisplayAlert("Error", "Please Fill in all necessary information. Notes are optional", "OK");
            return;  // Stop further processing if no course name is entered
        }

        // Date validation
        DateTime startDate = courseStartDatePicker.Date;
        DateTime endDate = courseEndDatePicker.Date;
        if (startDate >= endDate)
        {
            await DisplayAlert("Error", "Course End Date must be later than Course Start Date.", "OK");
            return;  // Stop further processing if dates are invalid
        }

        if (oaEndDatePicker.Date <= oaStartDatePicker.Date)
        {
            await DisplayAlert("Error", "Objective Assessment end date must be after the start date.", "OK");
            return;
        }

        if (paEndDatePicker.Date <= paStartDatePicker.Date)
        {
            await DisplayAlert("Error", "Performance Assessment end date must be after the start date.", "OK");
            return;
        }

        // Get User Input for Course
        string courseStatus = courseStatusPicker.SelectedItem as string ?? string.Empty;
        string instructorName = instructorNameEntry.Text;
        string instructorPhone = instructorPhoneEntry.Text;
        string instructorEmail = instructorEmailEntry.Text;
        string oaName = oaNameEntry.Text;
        DateTime oaStartDate = oaStartDatePicker.Date;
        DateTime oaEndDate = oaEndDatePicker.Date;
        string paName = paNameEntry.Text;
        DateTime paStartDate = paStartDatePicker.Date;
        DateTime paEndDate = paEndDatePicker.Date;
        string notes = notesEditor.Text;

        // Check if this is a new course or an edit
        if (_editCourseId == 0) // If it's a new course
        {
            await _dbService.CreateCourse(new Course
            {
                TermId = _termId, // Assuming _termId is the ID of the current term
                CourseName = courseNameEntry.Text,
                Start = startDate,
                End = endDate,
                CourseStatus = courseStatus,
                Instructor = instructorName,
                Phone = instructorPhone,
                Email = instructorEmail,
                OaName = oaName,
                OaStart = oaStartDate,
                OaEnd = oaEndDate,
                PaName = paName,
                PaStart = paStartDate,
                PaEnd = paEndDate,
                Notes = notes
            });
        }
        else // Updating an existing course
        {
            await _dbService.UpdateCourse(new Course
            {
                CourseId = _editCourseId,
                TermId = _termId,
                CourseName = courseNameEntry.Text,
                Start = startDate,
                End = endDate,
                CourseStatus = courseStatus,
                Instructor = instructorName,
                Phone = instructorPhone,
                Email = instructorEmail,
                OaName = oaName,
                OaStart = oaStartDate,
                OaEnd = oaEndDate,
                PaName = paName,
                PaStart = paStartDate,
                PaEnd = paEndDate,
                Notes = notes
            });

            _editCourseId = 0; // Reset edit ID after update
        }

        // Clear inputs and refresh the list of courses
        courseNameEntry.Text = string.Empty;
        courseStartDatePicker.Date = DateTime.Now;
        courseEndDatePicker.Date = DateTime.Now;
        instructorNameEntry.Text = string.Empty;
        instructorPhoneEntry.Text = string.Empty;
        instructorEmailEntry.Text = string.Empty;
        oaNameEntry.Text = string.Empty;
        oaStartDatePicker.Date = DateTime.Now;
        oaEndDatePicker.Date = DateTime.Now;
        paNameEntry.Text = string.Empty;
        paStartDatePicker.Date = DateTime.Now;
        paEndDatePicker.Date = DateTime.Now;
        notesEditor.Text = string.Empty;

        coursesListView.ItemsSource = await _dbService.GetCoursesByTermId(_termId);
    }

    private async void coursesListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var course = (Course)e.Item;
        var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete", "View Details");

        switch (action)
        {
            case "View Details":
                await Navigation.PushAsync(new CourseView(course));
                break;

            case "Edit":
                courseNameEntry.Text = course.CourseName;
                courseStartDatePicker.Date = course.Start;
                courseEndDatePicker.Date = course.End;
                courseStatusPicker.SelectedItem = course.CourseStatus;
                instructorNameEntry.Text = course.Instructor;
                instructorPhoneEntry.Text = course.Phone;
                instructorEmailEntry.Text = course.Email;
                oaNameEntry.Text = course.OaName;
                oaStartDatePicker.Date = course.OaStart;
                oaEndDatePicker.Date = course.OaEnd;
                paNameEntry.Text = course.PaName;
                paStartDatePicker.Date = course.PaStart;
                paEndDatePicker.Date = course.PaEnd;
                notesEditor.Text = course.Notes;

                // Save any ID or key necessary to indicate this is an update
                _editCourseId = course.CourseId;

                break;
            case "Delete":
                await _dbService.DeleteCourse(course);
                // Refresh the courses list
                coursesListView.ItemsSource = await _dbService.GetCoursesByTermId(course.TermId);
                break;
        }
    }
}