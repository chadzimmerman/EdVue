using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using WguMauiMobileApplication.Models;


namespace WguMauiMobileApplication.Classes
{
    [Table("Note")]
    public class Note : INotifyPropertyChanged, ISearchable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _title = "";
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

        private string _body = "";
        public string Body
        {
            get => _body;
            set
            {
                if (_body != value)
                {
                    _body = value;
                    OnPropertyChanged(nameof(Body));
                }
            }
        }

        public int CourseId { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
