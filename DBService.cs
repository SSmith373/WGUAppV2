using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace WGUAppV2
{
    public class DBService
    {
        private const string DB_Name = "WGUApp.DB";
        private readonly SQLiteAsyncConnection _connection;

        public DBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));
            _connection.CreateTableAsync<Term>();
            _connection.CreateTableAsync<Course>();
        }

        public async Task<List<Term>> GetTerm()
        {
            return await _connection.Table<Term>().ToListAsync();
        }

        public async Task<Term> GetByTermId(int id)
        {
            return await _connection.Table<Term>().Where(x => x.TermId == id).FirstOrDefaultAsync();
        }

        public async Task Create(Term term)
        {
            await _connection.InsertAsync(term);
        }

        public async Task Update(Term term)
        {
            await _connection.UpdateAsync(term);
        }

        public async Task Delete(Term term)
        {
            await _connection.DeleteAsync(term);
        }

        public async Task<List<Course>> GetCoursesByTermId(int termId)
        {
            return await _connection.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public async Task<Course> GetCourseById(int courseId)
        {
            return await _connection.Table<Course>().Where(c => c.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task CreateCourse(Course course)
        {
            await _connection.InsertAsync(course);
        }

        public async Task UpdateCourse(Course course)
        {
            await _connection.UpdateAsync(course);
        }

        public async Task DeleteCourse(Course course)
        {
            await _connection.DeleteAsync(course);
        }

    }
}
