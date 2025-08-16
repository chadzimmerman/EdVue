using WguMauiMobileApplication.Models;
using WguMauiMobileApplication.Services;

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
}