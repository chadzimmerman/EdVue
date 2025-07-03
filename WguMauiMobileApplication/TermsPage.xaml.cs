namespace WguMauiMobileApplication;

public partial class TermsPage : ContentPage
{
	public TermsPage()
	{
		InitializeComponent();
        BindingContext = new TermsPageViewModel();
    }
}