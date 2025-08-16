using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using WguMauiMobileApplication.Models;

namespace WguMauiMobileApplication
{
    public class Term : INotifyPropertyChanged, ISearchable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private DateTime _startDate;
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

        private DateTime _endDate;
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

        //ISearchable Polymorphism

        public bool Matches(string searchTerm)
        {
            return Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
        }

        public string GetDisplaytext()
        {
            return Title;
        }

        public string DisplayText => GetDisplaytext();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
