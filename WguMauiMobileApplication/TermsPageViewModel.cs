using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
                    if (_selectedTerm != null)
                        _selectedTerm.PropertyChanged += SelectedTerm_PropertyChanged;

                    
                    _ = LoadCoursesForTerm(_selectedTerm?.Id ?? 0);
                    
                }
            }
        }

        public ICommand SelectTermCommand { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand NavigateToCourseCommand { get; }



        public TermsPageViewModel()
        {
            SelectTermCommand = new Command<Term>(term =>
            {
                SelectedTerm = term;
            });

            AddCourseCommand = new Command(async () => await AddCourse());

            NavigateToCourseCommand = new Command<Course>(async (selectedCourse) =>
            {
                if (selectedCourse == null)
                {
                    Debug.WriteLine("SelectedCourse is null");
                    return;
                }

                await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?CourseId={selectedCourse.Id}");
            });

            _ = LoadTerms();
            //if you accidentally add new terms, uncomment this and run it once.
            //_ = ResetDataAndLoad(); 

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

        //temp delete the existing courses and allow them to reset to 6
        private async Task ResetDataAndLoad()
        {
            await DatabaseService.ResetTermsToDefaultAsync();
            await LoadTerms(); 
        }

        private async Task AddCourse()
        {
            if (SelectedTerm == null)
            {
                return;
            }
            var course = new Course()
            {
                TermId = SelectedTerm.Id,
                Name = "New Course",
                Code = "CXXX",
                Status = "Open"
            };

            await DatabaseService.AddCourseAsync(course);
            await LoadCoursesForTerm(SelectedTerm.Id);
        }


        private async Task LoadCoursesForTerm(int termId)
        {
            var courseList = await DatabaseService.GetCoursesByTermAsync(termId);
            Courses.Clear();
            foreach (var course in courseList)
                Courses.Add(course);
        }

        private void SelectedTerm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Term.Title) || e.PropertyName == nameof(Term.StartDate) || e.PropertyName == nameof(Term.EndDate))
            {
                // When the SelectedTerm properties change, update the database
                var term = sender as Term;
                if (term != null)
                {
                    // Save changes asynchronously but fire and forget here (or you can await elsewhere)
                    _ = DatabaseService.UpdateTermAsync(term);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
