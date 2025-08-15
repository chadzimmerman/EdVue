using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication.Pages;

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

    private void OnTermButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is TermsPageViewModel vm &&
        sender is Button button &&
        button.BindingContext is Term term)
        {
            vm.SelectedTerm = term;
        }
    }

    private void StartDate_Unfocused(object sender, FocusEventArgs e)
    {
        var vm = BindingContext as TermsPageViewModel;
        var term = vm?.SelectedTerm;
        if (term != null && term.StartDate > term.EndDate)
        {
            DisplayAlert("Invalid Dates", "Start date cannot be after end date.", "OK");
            term.EndDate = term.StartDate;
        }
    }

    private void EndDate_Unfocused(object sender, FocusEventArgs e)
    {
        var vm = BindingContext as TermsPageViewModel;
        var term = vm?.SelectedTerm;
        if (term != null && term.EndDate < term.StartDate)
        {
            DisplayAlert("Invalid Dates", "End date cannot be before start date.", "OK");
            term.StartDate = term.EndDate;
        }
    }



}