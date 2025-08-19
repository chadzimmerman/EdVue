using WguMauiMobileApplication.Models;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication.Pages
{
    public partial class ReportsPage : ContentPage
    {
        public ReportsPage()
        {
            InitializeComponent();
        }

        private async void GenerateReport_Clicked(object sender, EventArgs e)
        {
            var reportItems = new List<CourseReport>();

            var terms = await DatabaseService.GetTermsAsync();
            foreach (var term in terms)
            {
                var courses = await DatabaseService.GetCoursesByTermAsync(term.Id);
                foreach (var course in courses)
                {
                    reportItems.Add(new CourseReport
                    {
                        Term = term.Title,
                        Course = course.Name,
                        Status = course.Status
                    });
                }
            }

            ReportCollectionView.ItemsSource = reportItems;
            ReportTimestampLabel.Text = $"Generated on: {DateTime.Now:G}";
        }
    }
}
