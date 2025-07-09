using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication;

public partial class TermsPage : ContentPage
{
	public TermsPage()
	{
		InitializeComponent();
        BindingContext = new TermsPageViewModel();
    }

	private async void TermTitleEntry_Unfocused(object sender, FocusEventArgs e)
	{
		if (BindingContext is TermsPageViewModel vm && vm.SelectedTerm != null)
		{
			await DatabaseService.UpdateTermAsync(vm.SelectedTerm);
		}
	}
}