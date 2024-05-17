namespace WGUAppV2
{
    public partial class MainPage : ContentPage
    {
        private readonly DBService _dbService;
        private int _editTermId;
        public bool IsAddTermVisible { get; set; } = false;

        public MainPage(DBService dBService)
        {
            InitializeComponent();
            _dbService = dBService;
            BindingContext = this;
            Task.Run(async () => termListView.ItemsSource = await _dbService.GetTerm());
        }

        private void ToggleAddTermSection(object sender, EventArgs e)
        {
            IsAddTermVisible = !IsAddTermVisible;
            OnPropertyChanged(nameof(IsAddTermVisible));
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(termNameEntryField.Text))
            {
                await DisplayAlert("Error", "Please enter a Term Name.", "OK");
                return;  // Stop further processing
            }

            DateTime startDate = termStartPicker.Date;
            DateTime endDate = termEndPicker.Date;

            if (startDate >= endDate)
            {
                await DisplayAlert("Error", "End Date must be later than start date.", "OK");
                return;
            }

            if (_editTermId == 0)
            {
                await _dbService.Create(new Term
                {
                    TermName = termNameEntryField.Text,
                    Start = termStartPicker.Date,
                    End = termEndPicker.Date,
                });
            }

            else
            {
                await _dbService.Update(new Term
                {
                    TermId = _editTermId,
                    TermName = termNameEntryField.Text,
                    Start = termStartPicker.Date,
                    End = termEndPicker.Date,
                });

                _editTermId = 0;
            }

            termNameEntryField.Text = string.Empty;
            termStartPicker.Date = DateTime.Now;
            termEndPicker.Date = DateTime.Now;

            termListView.ItemsSource = await _dbService.GetTerm();

            // Collapse the Add Term section
            IsAddTermVisible = false;
            OnPropertyChanged(nameof(IsAddTermVisible));

        }

        private async void termListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var term = (Term)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete", "View Details");

            switch (action)
            {
                case "Edit":

                    IsAddTermVisible = true; // Expand the Add Term section
                    OnPropertyChanged(nameof(IsAddTermVisible));

                    _editTermId = term.TermId;
                    termNameEntryField.Text = term.TermName;
                    termStartPicker.Date = term.Start;
                    termEndPicker.Date = term.End;
                    break;

                case "Delete":

                    await _dbService.Delete(term);
                    termListView.ItemsSource = await _dbService.GetTerm();
                    break;

                case "View Details":
                    await Navigation.PushAsync(new TermView(term.TermId, term.TermName, term.Start, term.End, _dbService));
                    break;
            }
        }

        private async void OnEvaluationDataClicked(object sender, EventArgs e)
        {
            // Create a term
            var term = new Term
            {
                TermName = "Spring Term",
                Start = new DateTime(2024, 6, 1),
                End = new DateTime(2024, 6, 30)
            };
            await _dbService.Create(term);

            // Create a course within that term
            var course = new Course
            {
                TermId = term.TermId,
                CourseName = "Mobile App Class",
                Start = new DateTime(2024, 6, 1),
                End = new DateTime(2024, 6, 30),
                CourseStatus = "In Progress",
                Instructor = "Anika Patel",
                Phone = "555-123-4567",
                Email = "anika.patel@strimeuniversity.edu",
                OaName = "OA 1",
                OaStart = new DateTime(2024, 6, 20),
                OaEnd = new DateTime(2024, 6, 25),
                PaName = "PA 1",
                PaStart = new DateTime(2024, 6, 20),
                PaEnd = new DateTime(2024, 6, 25),
                Notes = "Here are my example notes"
            };
            await _dbService.CreateCourse(course);

            // Display a message to the user
            await DisplayAlert("Success", "Evaluation data has been added.", "OK");

            // Refresh the term list to include the new term
            termListView.ItemsSource = await _dbService.GetTerm();
        }

    }

}
