using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SQLite;
using WguMauiMobileApplication.Models;

namespace WguMauiMobileApplication
{
    public class Course : INotifyPropertyChanged, ISearchable
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int InstructorId { get; set; }
        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        private string code;
        public string Code
        {
            get => code;
            set
            {
                if (code != value)
                {
                    code = value;
                    OnPropertyChanged(nameof(Code));
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string coursePageRoute;
        public string CoursePageRoute
        {
            get => coursePageRoute;
            set
            {
                if (coursePageRoute != value)
                {
                    coursePageRoute = value;
                    OnPropertyChanged(nameof(CoursePageRoute));
                }
            }
        }

        private int termId;
        public int TermId
        {
            get => termId;
            set
            {
                if (termId != value)
                {
                    termId = value;
                    OnPropertyChanged(nameof(TermId));
                }
            }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate = DateTime.Now.AddMonths(6);
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public bool IsActive
        {
            get => Status == "Open";
            set
            {
                Status = value ? "Open" : "Completed";
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public string Details { get; set; }

        //ISearchable Polymorphism

        public bool Matches(string searchTerm)
        {
            return Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
        }

        public string GetDisplaytext()
        {
            return Name;
        }

        public string DisplayText => GetDisplaytext();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
