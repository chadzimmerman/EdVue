using WguMauiMobileApplication.Services;
using WguMauiMobileApplication.Classes;
using __XamlGeneratedCode__;

namespace WguMauiMobileApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            _ = InitAsync();
            MainPage = new AppShell();

        }

        private async Task InitAsync()
        {
            await DatabaseService.Init();
        }
    }
}
