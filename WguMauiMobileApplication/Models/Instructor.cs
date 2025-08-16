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
    [Table("Instructor")]
    public class Instructor : INotifyPropertyChanged, ISearchable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
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

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

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

