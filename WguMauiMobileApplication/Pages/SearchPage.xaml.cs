using WguMauiMobileApplication.Models;
namespace WguMauiMobileApplication.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
	}
	private List<ISearchable> allSearchItems;
	private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
	{
		string searchTerm = e.NewTextValue;

		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			resultsListView.ItemsSource = null; // or all items if you want
			return;
		}

		var filteredResults = allSearchItems
			.Where(item => item.Matches(searchTerm))
			.ToList();

		resultsListView.ItemsSource = filteredResults;
	}
}