using NUnit.Framework;
using System.Threading.Tasks;
using SQLite;
using WguAndroid.Services; // <-- Make sure this matches your namespace

namespace WguAndroid.Tests
{
    public class DatabaseServiceTests
    {
        private DatabaseService _db;

        [SetUp]
        public async Task Setup()
        {
            _db = new DatabaseService(new SQLiteAsyncConnection(":memory:"));
            await _db.Init();
        }

        [Test]
        public async Task AddStudent_Should_SaveStudent()
        {
            var student = new Student { Name = "Test Student" };

            await _db.SaveStudentAsync(student);
            var students = await _db.GetStudentsAsync();

            Assert.That(students.Count, Is.EqualTo(1));
            Assert.That(students[0].Name, Is.EqualTo("Test Student"));
        }

        [Test]
        public async Task AddCourse_ShouldSaveAndRetrieve()
        {
            var course = new Course { Name = "C971 Mobile Dev", InstructorEmail = "prof@wgu.edu" };
            await _db.SaveCourseAsync(course);

            var courses = await _db.GetCoursesAsync();
            Assert.AreEqual(1, courses.Count);
            Assert.AreEqual("C971 Mobile Dev", courses[0].Name);
        }

        [Test]
        public async Task AddAssessment_ShouldSaveAndRetrieve()
        {
            var assessment = new Assessment { Name = "Performance Assessment", Type = "Performance" };
            await _db.SaveAssessmentAsync(assessment);

            var assessments = await _db.GetAssessmentsAsync();
            Assert.AreEqual(1, assessments.Count);
            Assert.AreEqual("Performance Assessment", assessments[0].Name);
        }

        [Test]
        public async Task DeleteTerm_ShouldCascadeDeleteCoursesAndAssessments()
        {
            var term = new Term { Name = "Spring 2025", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) };
            await _db.SaveTermAsync(term);

            var course = new Course { Name = "Mobile Dev", InstructorEmail = "prof@wgu.edu", TermId = term.Id };
            await _db.SaveCourseAsync(course);

            var assessment = new Assessment { Name = "Final Project", Type = "Performance", CourseId = course.Id };
            await _db.SaveAssessmentAsync(assessment);

            // Delete term
            await _db.DeleteTermAsync(term);

            var terms = await _db.GetTermsAsync();
            var courses = await _db.GetCoursesAsync();
            var assessments = await _db.GetAssessmentsAsync();

            Assert.AreEqual(0, terms.Count);
            Assert.AreEqual(0, courses.Count);
            Assert.AreEqual(0, assessments.Count);
        }

        [Test]
        public void InvalidInstructorEmail_ShouldNotSave()
        {
            var course = new Course { Name = "Bad Email Course", InstructorEmail = "invalidemail" };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _db.SaveCourseAsync(course);
            });
        }

    }
}
