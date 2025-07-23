using System.Collections.ObjectModel;
using WguMauiMobileApplication.Classes;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication;

public partial class CourseAssessmentsView : ContentView
{
	public ObservableCollection<Assessment> Assessments { get; set; } = new();

	private int _courseId;
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

		var newAssessment = new Assessment
		{
			CourseId = _courseId,
			Name = AssessmentNameEntry.Text,
			Type = AssessmentTypePicker.SelectedItem.ToString(),
			StartDate = AssessmentStartDate.Date,
			EndDate = AssessmentEndDate.Date,
			Notify = NotifySwitch.IsToggled
		};

		await DatabaseService.SaveAssessmentAsync(newAssessment);

		Assessments.Add(newAssessment);

		//TODO: if notify is true, schedule notification here for the notifications tab
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
}