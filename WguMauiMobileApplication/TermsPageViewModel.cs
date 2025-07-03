using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace WguMauiMobileApplication
{
    public class TermsPageViewModel
    {
        // Collection of courses for the UI to show
        public ObservableCollection<Course> Courses { get; set; }

        // Navigation command for your buttons
        public ICommand NavigateToCourseCommand { get; }

        public TermsPageViewModel()
        {
            // Initialize some sample data
            Courses = new ObservableCollection<Course>
        {
            new Course { Status = "Active", Code = "C971", Name = "Mobile Application Development", CoursePageRoute = "CourseDetailPage" },
            new Course { Status = "Passed", Code = "C168", Name = "Data Structures & Algorithms", CoursePageRoute = "CourseDetailPage" }
        };

            NavigateToCourseCommand = new Command<Course>(course =>
            {
                // Here you put your navigation logic, for example:
                Shell.Current.GoToAsync($"{course.CoursePageRoute}?code={course.Code}");
            });
        }
    }
}
