using WguMauiMobileApplication.Pages;

namespace WguMauiMobileApplication

{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CoursesPage), typeof(CoursesPage));
            Routing.RegisterRoute(nameof(InstructorPage), typeof(InstructorPage));
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        }
    }
}
