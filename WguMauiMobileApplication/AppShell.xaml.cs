namespace WguMauiMobileApplication
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CoursesPage), typeof(CoursesPage));
        }
    }
}
