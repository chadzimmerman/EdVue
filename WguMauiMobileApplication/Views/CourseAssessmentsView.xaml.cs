using System.Collections.ObjectModel;
using Plugin.LocalNotification;
using WguMauiMobileApplication.Models;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication;

public partial class CourseAssessmentsView : ContentView
{
    public ObservableCollection<Assessment> Assessments { get; set; } = new();

    private int _courseId;
    private int? editingAssessmentId = null;

    public CourseAssessmentsView()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public async void LoadForCourse(int courseId)
    {
        _courseId = courseId;
        var assessments = await DatabaseService.GetAssessmentsByCourseIdAsync(courseId);
        Assessments.Clear();
        foreach (var a in assessments)
        {
            Assessments.Add(a);
        }
    }

    private async void OnAddAssessmentClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AssessmentNameEntry.Text) ||
            AssessmentTypePicker.SelectedIndex == -1)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Please enter a name and select a type", "OK");
            return;
        }

        if (AssessmentStartDate.Date > AssessmentEndDate.Date)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Assessment start date cannot be after end date", "OK");
            return;
        }

        // var newAssessment = new Assessment
        // {
        //     CourseId = _courseId,
        //     Name = AssessmentNameEntry.Text,
        //     Type = AssessmentTypePicker.SelectedItem.ToString(),
        //     StartDate = AssessmentStartDate.Date,
        //     EndDate = AssessmentEndDate.Date,
        //     Notify = NotifySwitch.IsToggled
        // };

        // await DatabaseService.SaveAssessmentAsync(newAssessment);

        // Assessments.Add(newAssessment);

        Assessment assessment;

        if (editingAssessmentId.HasValue)
        {
            // Update existing assessment
            assessment = Assessments.First(a => a.Id == editingAssessmentId.Value);
            assessment.Name = AssessmentNameEntry.Text;
            assessment.Type = AssessmentTypePicker.SelectedItem.ToString();
            assessment.StartDate = AssessmentStartDate.Date;
            assessment.EndDate = AssessmentEndDate.Date;
            assessment.Notify = NotifySwitch.IsToggled;

            await DatabaseService.SaveAssessmentAsync(assessment);

            // Reset editing
            editingAssessmentId = null;
            AddAssessmentButton.Text = "Add Assessment";
        }
        else
        {
            // Add new assessment
            assessment = new Assessment
            {
                CourseId = _courseId,
                Name = AssessmentNameEntry.Text,
                Type = AssessmentTypePicker.SelectedItem.ToString(),
                StartDate = AssessmentStartDate.Date,
                EndDate = AssessmentEndDate.Date,
                Notify = NotifySwitch.IsToggled
            };

            await DatabaseService.SaveAssessmentAsync(assessment);
            Assessments.Add(assessment);
        }

        // Refresh CollectionView
        AssessmentsListView.ItemsSource = null;
        AssessmentsListView.ItemsSource = Assessments;

        // Clear form
        AssessmentNameEntry.Text = string.Empty;
        AssessmentTypePicker.SelectedIndex = -1;
        NotifySwitch.IsToggled = false;

        if (assessment.Notify)
        {
            var startNotification = new NotificationRequest
            {
                Title = "Assessment Starting",
                Description = $"{assessment.Name} starts today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = assessment.StartDate,
                    ////NotifyTime = DateTime.Now.AddSeconds(10),
                    RepeatType = NotificationRepeat.No
                }
            };

            var endNotification = new NotificationRequest
            {
                Title = "Assessment Due",
                Description = $"{assessment.Name} is due today!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = assessment.EndDate,
                    RepeatType = NotificationRepeat.No
                }
            };

            await LocalNotificationCenter.Current.Show(startNotification);
            await LocalNotificationCenter.Current.Show(endNotification);
        }

    }

    private async void OnDeleteAssessmentClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var id = (int)button.CommandParameter;
        var toRemove = Assessments.FirstOrDefault(a => a.Id == id);
        if (toRemove != null)
        {
            await DatabaseService.DeleteAssessmentAsync(toRemove);
            Assessments.Remove(toRemove);
        }
    }

    private void OnEditAssessmentClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var id = (int)button.CommandParameter;
        var toEdit = Assessments.FirstOrDefault(a => a.Id == id);

        if (toEdit != null)
        {
            AssessmentNameEntry.Text = toEdit.Name;
            AssessmentTypePicker.SelectedItem = toEdit.Type;
            AssessmentStartDate.Date = toEdit.StartDate;
            AssessmentEndDate.Date = toEdit.EndDate;
            NotifySwitch.IsToggled = toEdit.Notify;

            // Store the ID for update later
            editingAssessmentId = toEdit.Id;
            AddAssessmentButton.Text = "Update Assessment";
        }
    }

    private async void OnDateChanged(object sender, DateChangedEventArgs e)
    {
        if (BindingContext is CoursesPageViewModel vm && vm.SelectedCourse is Course course)
        {
            if (course.StartDate > course.EndDate)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Invalid Dates",
                    "Course start date cannot be after the end date.",
                    "OK");

                course.EndDate = course.StartDate;
                return;
            }

            if (course.EndDate < course.StartDate)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Invalid Dates",
                    "Course end date cannot be before the start date.",
                    "OK");

                course.StartDate = course.EndDate;
                return;
            }

            await DatabaseService.UpdateCourseAsync(course);
        }
    }
}