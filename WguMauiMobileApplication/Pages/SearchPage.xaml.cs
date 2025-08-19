using WguMauiMobileApplication.Models;
using WguMauiMobileApplication.Services;
using WguMauiMobileApplication.Pages;

namespace WguMauiMobileApplication.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
	}
	private List<ISearchable> allSearchItems;
	private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		string searchTerm = e.NewTextValue;

		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			resultsListView.ItemsSource = null;
			return;
		}

		var allSearchItems = new List<ISearchable>();

		allSearchItems.AddRange(await DatabaseService.GetTermsAsync());
		allSearchItems.AddRange(await DatabaseService.GetAllCoursesAsync());
		allSearchItems.AddRange(await DatabaseService.GetAllInstructorsAsync());
		allSearchItems.AddRange(await DatabaseService.GetAllNotesAsync());


		var filteredResults = allSearchItems
			.Where(item => item.Matches(searchTerm))
			.ToList();

		resultsListView.ItemsSource = filteredResults;
	}

	private async void SearchResultsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		if (e.SelectedItem == null)
			return;

		var selectedItem = e.SelectedItem;

		// Navigate based on the type
		switch (selectedItem)
		{
			case Term term:
				await Shell.Current.GoToAsync($"{nameof(TermsPage)}?termId={term.Id}");
				break;

			case Course course:
				await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?courseId={course.Id}");
				break;

			case Instructor instructor:
				await Shell.Current.GoToAsync($"{nameof(InstructorPage)}?instructorId={instructor.Id}");
				break;

			case Note note:
				await Shell.Current.GoToAsync($"{nameof(NotesPage)}?noteId={note.Id}");
				break;
		}

		// Optional: clear selection so item doesn't stay highlighted
		((ListView)sender).SelectedItem = null;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		searchBar.Text = string.Empty;
		resultsListView.ItemsSource = null;
	}

}