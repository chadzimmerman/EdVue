using Plugin.LocalNotification;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication.Pages;
[QueryProperty(nameof(CourseId), "courseId")]
public partial class DetailsPage : ContentPage
{
	private Course _course;
    private int courseId;
    public int CourseId
    {
        get => courseId;
            set
        {
            courseId = value;
            LoadCourseDetails(courseId);
        }
    }
	public DetailsPage()
	{
		InitializeComponent();
    }

    private async void LoadCourseDetails(int courseId)
    {
        _course = await DatabaseService.GetCourseByIdAsync(courseId);
        if (_course != null)
        {
            TitleEntry.Text = _course.Name;
            StartDatePicker.Date = _course.StartDate;
            EndDatePicker.Date = _course.EndDate;
            DetailsEditor.Text = _course.Details;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _course.Name = TitleEntry.Text;
        _course.StartDate = StartDatePicker.Date;
        _course.EndDate = EndDatePicker.Date;
        _course.Details = DetailsEditor.Text;

        await DatabaseService.UpdateCourseAsync(_course);
        await DisplayAlert("Saved", "Course information saved successfully.", "OK");
        await Navigation.PopAsync();  // Go back
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this course?", "Yes", "No");
        if (!confirm) return;

        await DatabaseService.DeleteCourseAsync(_course);
        await DisplayAlert("Deleted", "Course deleted.", "OK");
        await Navigation.PopAsync();  // Go back
    }

    private void OnSetAlertsClicked(object sender, EventArgs e)
    {
        if (_course == null) return;

        var startNotification = new NotificationRequest
        {
            NotificationId = _course.Id * 10 + 1,
            Title = "Course Starting",
            Description = $"Your course starts today!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = _course.StartDate.Date, //set for 12:00AM
                NotifyRepeatInterval = TimeSpan.FromDays(1),
                RepeatType = NotificationRepeat.No
            }
        };

        var endNotification = new NotificationRequest
        {
            NotificationId = _course.Id * 10 + 2,
            Title = "Course Ending",
            Description = $"Your course '{_course.Name} ends today!",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = _course.EndDate.Date,
                NotifyRepeatInterval = TimeSpan.FromDays(1),
                RepeatType = NotificationRepeat.No
            }
        };

        LocalNotificationCenter.Current.Show(startNotification);
        LocalNotificationCenter.Current.Show(endNotification);

        DisplayAlert("Alerts Set", "Notifications scheduled for course start and end.", "OK");
    }
}