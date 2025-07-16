using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            _ = DatabaseService.Init();
            MainPage = new AppShell();
            //MainPage = new TermsPage();
            //MainPage = new CoursesPage();

        }
    }
}
