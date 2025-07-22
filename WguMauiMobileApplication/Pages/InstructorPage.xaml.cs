using WguMauiMobileApplication.Classes;
using WguMauiMobileApplication.Services;


namespace WguMauiMobileApplication.Pages;

[QueryProperty(nameof(InstructorId), "instructorId")]
[QueryProperty(nameof(CourseId), "courseId")]
public partial class InstructorPage : ContentPage
{
	private Instructor _instructor;
	private int _instructorId;
    private int _courseId;
    private bool _isLoaded = false;
    public int InstructorId
	{
		get => _instructorId;
		set
		{
            if (_isLoaded) return;
            _instructorId = value;
			LoadInstructor(value);
            _isLoaded = true;
        }
	}
    public int CourseId
    {
        get => _courseId;
        set
        {
            _courseId = value;
            TryLoad();
        }
    }
    private void TryLoad()
    {
        if (_isLoaded || _courseId == 0) return;

        _isLoaded = true;
        LoadInstructor(_instructorId);
    }
    public InstructorPage()
	{
		InitializeComponent();
	}

    private async void LoadInstructor(int id)
    {
        _instructor = await DatabaseService.GetInstructorByIdAsync(id);
		if (_instructor == null)
		{
            _instructor = new Instructor
            {
                Id = id,
                Name = "",
                Email = "",
                Phone = ""
            };
            await DisplayAlert("Error", "Instructor not found.", "OK");
		}
        BindingContext = _instructor;
    }

    private async void OnSaveInstructorClicked(object sender, EventArgs e)
    {
        if (_instructor == null)
        {
            await DisplayAlert("Error", "Instructor not loaded", "OK");
            return;
        }

        if (string.IsNullOrEmpty(_instructor.Name) ||
            string.IsNullOrEmpty(_instructor.Email) ||
            string.IsNullOrEmpty(_instructor.Phone) ||
            !_instructor.Email.Contains("@"))
        {
            await DisplayAlert("Validation Error", "Please enter a valid name and email.", "OK");
            return;
        }

        var isNew = _instructor.Id == 0;

        await DatabaseService.UpdateInstructorAsync(_instructor);

        if (isNew && _courseId != 0)
        {
            var course = await DatabaseService.GetCourseByIdAsync(_courseId);
            if (course != null)
            {
                course.InstructorId = _instructor.Id;
                await DatabaseService.UpdateCourseAsync(course);
            }
        }

        await DisplayAlert("Success", "Instructor information saved.", "OK");

     
    }
}