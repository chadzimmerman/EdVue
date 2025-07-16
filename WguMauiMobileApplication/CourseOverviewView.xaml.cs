using WguMauiMobileApplication.Services;

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
}