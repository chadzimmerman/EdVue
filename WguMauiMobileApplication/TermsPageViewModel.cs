using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication
{
    public class TermsPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Term> Terms { get; set; } = new();
        public ObservableCollection<Course> Courses { get; set; } = new();

        private Term _selectedTerm;
        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                if (_selectedTerm != value)
                {
                    _selectedTerm = value;
                    OnPropertyChanged(nameof(SelectedTerm));
                    LoadCoursesForTerm(_selectedTerm?.Id ?? 0);
                }
            }
        }

        public ICommand SelectTermCommand { get; }
        public ICommand AddTermCommand { get; }

        public TermsPageViewModel()
        {
            SelectTermCommand = new Command<Term>(term =>
            {
                SelectedTerm = term;
            });

            AddTermCommand = new Command(async () =>
            {
                var newTerm = new Term
                {
                    Title = $"Term {DateTime.Now.Ticks % 10000}",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(6)
                };
                await DatabaseService.AddTermAsync(newTerm);
                await LoadTerms();
            });

            _ = LoadTerms(); // Load on init
        }

        public async Task LoadTerms()
        {
            await DatabaseService.Init();
            var list = await DatabaseService.GetTermsAsync();

            Terms.Clear();
            foreach (var term in list)
                Terms.Add(term);

            // Optional: auto-select first term
            if (Terms.Any())
                SelectedTerm = Terms.Last();
        }



        private async void LoadCoursesForTerm(int termId)
        {
            var courseList = await DatabaseService.GetCoursesByTermAsync(termId);
            Courses.Clear();
            foreach (var course in courseList)
                Courses.Add(course);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
