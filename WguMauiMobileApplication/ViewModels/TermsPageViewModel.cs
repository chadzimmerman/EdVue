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
using WguMauiMobileApplication.Pages;

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
        public ICommand AddTermCommand { get; }
        public ICommand DeleteSelectedTermCommand { get; }


        public TermsPageViewModel()
        {
            SelectTermCommand = new Command<Term>(term =>
            {
                SelectedTerm = term;
            });

            AddCourseCommand = new Command(async () => await AddCourse());
            DeleteSelectedTermCommand = new Command(async () => await DeleteSelectedTerm());

            AddTermCommand = new Command(async () => await AddTerm());

            NavigateToCourseCommand = new Command<Course>(async (selectedCourse) =>
            {
                if (selectedCourse == null)
                {
                    Debug.WriteLine("SelectedCourse is null");
                    return;
                }

                await Shell.Current.GoToAsync($"{nameof(CoursesPage)}?CourseId={selectedCourse.Id}", true);
            });

            _ = LoadTerms();
        }
        public TermsPageViewModel(int termId)
        {
            Terms = new ObservableCollection<Term>();
            Courses = new ObservableCollection<Course>();

            _ = LoadSingleTerm(termId);
        }
        private async Task LoadSingleTerm(int termId)
        {
            var term = await DatabaseService.GetTermByIdAsync(termId);
            if (term != null)
            {
                Terms.Clear();
                Terms.Add(term);
                SelectedTerm = term; 
            }
        }


        private async Task SeedEvaluationDataAndLoad()
        {
            await DatabaseService.Init();
            await DatabaseService.SeedDemoData(); // move this logic to a place DatabaseService can call?
            await LoadTerms();
        }

        public async Task LoadTerms()
        {
            await DatabaseService.Init();
            var list = await DatabaseService.GetTermsAsync();

            Terms.Clear();
            foreach (var term in list)
                Terms.Add(term);

            if (Terms.Any())
                SelectedTerm = Terms.Last();
        }

        private async Task DeleteSelectedTerm()
        {
            if (SelectedTerm == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Term",
                $"Are you sure you want to delete \"{SelectedTerm.Title}\"?",
                "Yes", "No");

            if (confirm)
            {
                await DatabaseService.DeleteTermAsync(SelectedTerm);
                await LoadTerms();
            }
        }

        private async Task AddTerm()
        {
            var term = new Term
            {
                Title = $"Term {Terms.Count + 1}",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(6)
            };

            await DatabaseService.AddTermAsync(term);
            await LoadTerms();
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

        private async void SelectedTerm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var term = sender as Term;
            if (term == null) return;

            if (e.PropertyName == nameof(Term.Title) ||
                e.PropertyName == nameof(Term.StartDate) ||
                e.PropertyName == nameof(Term.EndDate))
            {
                if (term.StartDate > term.EndDate)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Invalid Dates",
                        "Start date cannot be after end date.",
                        "OK");
                    term.EndDate = term.StartDate;
                    return;
                }

                if (term.EndDate < term.StartDate)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Invalid Dates",
                        "End date cannot be before start date.",
                        "OK");
                    term.StartDate = term.EndDate;
                    return;
                }
            }
            if (e.PropertyName == nameof(Term.Title) ||
                e.PropertyName == nameof(Term.StartDate) ||
                e.PropertyName == nameof(Term.EndDate))
            {
                await DatabaseService.UpdateTermAsync(term);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
