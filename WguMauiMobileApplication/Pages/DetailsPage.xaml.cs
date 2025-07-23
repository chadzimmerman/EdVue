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
        var course = await DatabaseService.GetCourseByIdAsync(courseId);
        if (course != null)
        {
            // Assuming you have UI elements like TitleEntry, StartDatePicker, etc.
            TitleEntry.Text = course.Name;
            StartDatePicker.Date = course.StartDate;
            EndDatePicker.Date = course.EndDate;
            DetailsEditor.Text = course.Details;
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
}