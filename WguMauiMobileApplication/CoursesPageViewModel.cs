using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication
{
    public class CoursesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ShowOverviewCommand { get; }
        public ICommand ShowAssessmentsCommand { get; }
        private Course _selectedcourse;
        public Course SelectedCourse
        {
            get => _selectedcourse;
            set
            {
                if (_selectedcourse != value)
                {
                    _selectedcourse = value;
                    OnPropertyChanged(nameof(SelectedCourse));
                }
            }
        }

        private bool _isOverviewSelected = true;
        public bool IsOverviewSelected
        {
            get => _isOverviewSelected;
            set
            {
                if (_isOverviewSelected != value)
                {
                    _isOverviewSelected = value;
                    OnPropertyChanged(nameof(IsOverviewSelected));
                    OnTabChanged?.Invoke();
                }
            }
        }

        public Action? OnTabChanged { get; set;}

        public CoursesPageViewModel(Course course)
        {
            SelectedCourse = course;
            if (SelectedCourse != null)
            {
                
                SelectedCourse.PropertyChanged += async (sender, args) =>
                {
                    if ((args.PropertyName is "Name" or "Status" or "Code" or "StateDate" or "EndDate"))
                    {
                        await DatabaseService.UpdateCourseAsync(SelectedCourse);
                        Debug.WriteLine($"✅ Course updated due to {args.PropertyName}");
                    }
                };
            }
            
            ShowOverviewCommand = new Command(() => IsOverviewSelected = true);
            ShowAssessmentsCommand = new Command(() => IsOverviewSelected = false);
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
