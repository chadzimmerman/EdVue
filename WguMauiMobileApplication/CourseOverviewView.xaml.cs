using WguMauiMobileApplication.Services;
using WguMauiMobileApplication.Pages;

namespace WguMauiMobileApplication;

public partial class CourseOverviewView : ContentView
{
	public CourseOverviewView()
	{
		InitializeComponent();
	}

    private async void OnDateUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is CoursesPageViewModel vm && vm.SelectedCourse != null)
        {
            await DatabaseService.UpdateCourseAsync(vm.SelectedCourse);
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