using WguMauiMobileApplication.Services;
using WguMauiMobileApplication.Pages;

namespace WguMauiMobileApplication;

public partial class CourseOverviewView : ContentView
{
	public CourseOverviewView()
	{
		InitializeComponent();
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

    private async void OnInstructorNavClicked(object sender, EventArgs e)
    {
        if (BindingContext is CoursesPageViewModel vm && vm.SelectedCourse != null)
        {
            var instructorId = vm.SelectedCourse.InstructorId;
            await Shell.Current.GoToAsync($"{nameof(InstructorPage)}?instructorId={instructorId}&courseId={vm.SelectedCourse.Id}");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No course selected", "OK");
        }
    }

    private async void OnNotesNavClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotesPage));
    }

    private async void OnDetailsNavClicked(object sender, EventArgs e)
    {

        if (BindingContext is CoursesPageViewModel vm && vm.SelectedCourse != null)
        {
            var courseId = vm.SelectedCourse.Id;
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?courseId={courseId}");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No course selected", "OK");
        }
    }
}