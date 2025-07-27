using WguMauiMobileApplication.Services;
using WguMauiMobileApplication.Classes;

namespace WguMauiMobileApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //_ = DatabaseService.Init();
            _ = InitAsync();
            MainPage = new AppShell();
            //MainPage = new TermsPage();
            //MainPage = new CoursesPage();

        }

        private async Task InitAsync()
        {
            await DatabaseService.Init();
            await SeedDemoData();
        }

        private async Task SeedDemoData()
        {
            var existingTerm = await DatabaseService.GetTermByNameAsync("Evaluation Term");
            if (existingTerm != null)
            {
                var term = new Term
                {
                    Title = "Sample Term",
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.AddMonths(4).Date
                };
                await DatabaseService.AddTermAsync(term);

                var instructor = new Instructor
                {
                    Name = "Anika Patel",
                    Phone = "555-123-4567",
                    Email = "anika.patel@strimeuniversity.edu"
                };
                await DatabaseService.AddInstructorAsync(instructor);

                var course = new Course
                {
                    Name = "Evaluation Course",
                    Code = "CSE101",
                    Status = "Open",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddMonths(6),
                    TermId = term.Id,
                    InstructorId = instructor.Id
                };
                await DatabaseService.AddCourseAsync(course);

                var assessment1 = new Assessment
                {
                    CourseId = course.Id,
                    Name = "Performance Assessment",
                    Type = "Performance",
                    StartDate = DateTime.Now.AddDays(1).Date,
                    EndDate = DateTime.Now.AddDays(7).Date,
                    Notify = true,
                };

                var assessment2 = new Assessment
                {
                    CourseId = course.Id,
                    Name = "Objective Assessment",
                    Type = "Objective",
                    StartDate = DateTime.Now.AddDays(8).Date,
                    EndDate = DateTime.Now.AddDays(14).Date,
                    Notify = false
                };

                await DatabaseService.SaveAssessmentAsync(assessment1);
                await DatabaseService.SaveAssessmentAsync(assessment2);
            }
        }

    }
}
