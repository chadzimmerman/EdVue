using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WguMauiMobileApplication.Services;

namespace WguMauiMobileApplication
{
    public class TermsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Term> Terms { get; set; } = new();

        public ICommand AddTermCommand { get; }
        public ICommand DeleteTermCommand { get; }
        public ICommand EditTermCommand { get; }

        public TermsViewModel()
        {
            LoadTerms();

            AddTermCommand = new Command(async () =>
            {
                var newTerm = new Term
                {
                    Title = $"Term {DateTime.Now.Ticks % 10000}",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(6)
                };

                await DatabaseService.AddTermAsync(newTerm);
                LoadTerms();
            });

            DeleteTermCommand = new Command<Term>(async term =>
            {
                await DatabaseService.DeleteTermAsync(term);
                LoadTerms();
            });

            EditTermCommand = new Command<Term>(async term =>
            {
                // For now, just update title as an example
                term.Title += " (Edited)";
                await DatabaseService.UpdateTermAsync(term);
                LoadTerms();
            });
        }

        private async void LoadTerms()
        {
            await DatabaseService.Init();

            var termList = await DatabaseService.GetTermsAsync();
            Terms.Clear();

            foreach (var term in termList)
            {
                Terms.Add(term);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
