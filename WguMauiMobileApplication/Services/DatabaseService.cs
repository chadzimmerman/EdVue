using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using WguMauiMobileApplication.Classes;

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
            await _database.CreateTableAsync<Instructor>();
            await _database.CreateTableAsync<Note>();

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

        public static async Task ResetTermsToDefaultAsync()
        {
            await Init();

            var existingTerms = await _database.Table<Term>().ToListAsync();
            foreach (var terms in existingTerms)
            {
                await _database.DeleteAsync(terms);
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

        public static Task<Term> GetTermByNameAsync(string name)
        {
            return _database.Table<Term>()
                            .Where(t => t.Title == name)
                            .FirstOrDefaultAsync();
        }



        //Courses
        public static Task<List<Course>> GetCoursesByTermAsync(int termId) => 
            _database.Table<Course>().Where(c => c.TermId == termId).ToListAsync();

        public static Task<int> AddCourseAsync(Course course) =>
            _database.InsertAsync(course);

        public static Task<Course> GetCourseByIdAsync(int id) =>
            _database.Table<Course>().Where(c => c.Id == id).FirstOrDefaultAsync();

        public static Task<int> UpdateCourseAsync(Course course) =>
            _database.UpdateAsync(course);

        public static Task<int> DeleteCourseAsync(Course course) =>
            _database.DeleteAsync(course);


        //Instructors
        public static async Task<int> UpdateInstructorAsync(Instructor instructor)
        {
            if (instructor.Id == 0)
            {
                return await _database.InsertAsync(instructor);
            }
            else
            {
                var existing = await _database.Table<Instructor>().Where(i => i.Id == instructor.Id).FirstOrDefaultAsync();
                if (existing == null)
                {
                    return await _database.InsertAsync(instructor);
                }
                else
                {
                    return await _database.UpdateAsync(instructor);
                }
            }
        }
        public static Task<Instructor> GetInstructorByIdAsync(int id) =>
              _database.Table<Instructor>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public static async Task<int> AddInstructorAsync(Instructor instructor)
        {
            return await _database.InsertAsync(instructor);
        }

        //Assessments
        public static async Task<List<Assessment>> GetAssessmentsByCourseIdAsync(int courseId)
        {
            return await _database.Table<Assessment>()
                      .Where(a => a.CourseId == courseId)
                      .ToListAsync();
        }

        public static async Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.Id != 0)
            {
                return await _database.UpdateAsync(assessment);
            }
            else
            {
                return await _database.InsertAsync(assessment);
            }
        }

        public static async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return await _database.DeleteAsync(assessment);
        }

        //Notes
        public async Task<List<Note>> GetNotesAsync()
        {
            return await _database.Table<Note>().ToListAsync();
        }

        public async Task<int> AddNoteAsync(Note note)
        {
            return await _database.InsertAsync(note);
        }

        public async Task<int> UpdateNoteAsync(Note note)
        {
            return await _database.UpdateAsync(note);
        }

        public async Task<int> DeleteNoteAsync(Note note)
        {
            return await _database.DeleteAsync(note);
        }








        //eval data
        public static async Task SeedDemoData()
        {

            var existingTerm = await DatabaseService.GetTermByNameAsync("Evaluation Term");
            if (existingTerm == null)
            {
                var term = new Term
                {
                    Title = "Sample Term",
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.AddMonths(4).Date
                };
                await DatabaseService.AddTermAsync(term);

                var instructor = new Instructor
                {
                    Name = "Anika Patel",
                    Phone = "555-123-4567",
                    Email = "anika.patel@strimeuniversity.edu"
                };
                await DatabaseService.AddInstructorAsync(instructor);

                var course = new Course
                {
                    Name = "Evaluation Course",
                    Code = "CSE101",
                    Status = "Open",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddMonths(6),
                    TermId = term.Id,
                    InstructorId = instructor.Id
                };
                await DatabaseService.AddCourseAsync(course);

                var assessment1 = new Assessment
                {
                    CourseId = course.Id,
                    Name = "Performance Assessment",
                    Type = "Performance",
                    StartDate = DateTime.Now.AddDays(1).Date,
                    EndDate = DateTime.Now.AddDays(7).Date,
                    Notify = true,
                };

                var assessment2 = new Assessment
                {
                    CourseId = course.Id,
                    Name = "Objective Assessment",
                    Type = "Objective",
                    StartDate = DateTime.Now.AddDays(8).Date,
                    EndDate = DateTime.Now.AddDays(14).Date,
                    Notify = false
                };

                await DatabaseService.SaveAssessmentAsync(assessment1);
                await DatabaseService.SaveAssessmentAsync(assessment2);
            }
        }
    }
}
