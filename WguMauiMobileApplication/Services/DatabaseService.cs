using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace WguMauiMobileApplication.Services
{
    class DatabaseService
    {
        private static SQLiteAsyncConnection _database;

        public static async Task Init()
        {
            if ((_database != null))
            {
                return;
            }

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "wgu.db");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Term>();
            await _database.CreateTableAsync<Course>();

            var existingTerms = await _database.Table<Term>().ToListAsync();
            if (!existingTerms.Any())
            {
                var term1 = new Term
                {
                    Title = "Term 1",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(6)
                };
                await _database.InsertAsync(term1);

                var courses = new List<Course>
        {
            new Course { Name = "Mobile Application Dev", Code = "C971", Status = "Open", TermId = term1.Id },
            new Course { Name = "Data Structures", Code = "C173", Status = "InProgress", TermId = term1.Id }
        };

                foreach (var course in courses)
                    await _database.InsertAsync(course);
            }
        }
        //Terms
        public static Task<List<Term>> GetTermsAsync() =>
            _database.Table<Term>().ToListAsync();

        public static Task<int> AddTermAsync(Term term) =>
            _database.InsertAsync(term);

        public static Task<int> UpdateTermAsync(Term term) =>
            _database.UpdateAsync(term);

        public static Task<int> DeleteTermAsync(Term term) =>
            _database.DeleteAsync(term);

        //Courses

        public static Task<List<Course>> GetCoursesByTermAsync(int termId) => 
            _database.Table<Course>().Where(c => c.TermId == termId).ToListAsync();

        public static Task<int> AddCourseAsync(Course course) =>
            _database.InsertAsync(course);
    }
}
