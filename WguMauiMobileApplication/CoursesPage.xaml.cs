using System.Diagnostics;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication;

[QueryProperty(nameof(CoursesId), "CourseId")]
public partial class CoursesPage : ContentPage
{
    private int _courseId;
    public int CoursesId
    {
        get => _courseId;
        set
        {
            _courseId = value;
            LoadCourse(_courseId);
        }
    }
    private Course _selectedCourse;
    private CoursesPageViewModel ViewModel => (CoursesPageViewModel)BindingContext;
	public CoursesPage()
	{
		InitializeComponent();
        var vm = new CoursesPageViewModel(_selectedCourse);
        BindingContext = vm;
        vm.OnTabChanged = SwapTabContent;
        SwapTabContent(); 
    }

	private async void LoadCourse(int id)
	{
        await DatabaseService.Init();
        var course = await DatabaseService.GetCourseByIdAsync(id);
        if (course == null)
        {
            Debug.WriteLine("Course not found.");
            return;
        }

        var vm = new CoursesPageViewModel(course);
        vm.OnTabChanged = SwapTabContent;
        BindingContext = vm;
        SwapTabContent();

        Debug.WriteLine($"✅ Loaded course: {course.Name}, {course.Id}");
    }

    private void SwapTabContent()
    {
        if (ViewModel.IsOverviewSelected)
        {
            TabContentView.Content = new CourseOverviewView();
        }
        else
        {
            var assessmentsView = new CourseAssessmentsView();
            assessmentsView.LoadForCourse(ViewModel.SelectedCourse.Id);
            TabContentView.Content = assessmentsView;
        }

        //TabContentView.Content = ViewModel.IsOverviewSelected
        //    ? new CourseOverviewView()
        //    : new CourseAssessmentsView();
    }

    private async void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (BindingContext is CoursesPageViewModel vm && vm.SelectedCourse != null)
        {
            await DatabaseService.UpdateCourseAsync(vm.SelectedCourse);
        }
    }

}