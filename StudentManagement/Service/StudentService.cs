using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Service
{
    public class StudentService : IStudent
    {
        private readonly StudentDbContext _context;
        public StudentService(StudentDbContext dbContext) 
        {
            _context = dbContext;
        }

        public Student GetStudent(int id)
        {
            return _context.Students.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Student> GetStudents()
        {
            return _context.Students;
        }
        public void AddStudent(Student student)
        {
            if(student == null)
            {
                throw new ArgumentNullException("Not Found");
            }
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            var Oldstd = _context.Students.FirstOrDefault(x => x.Id == student.Id);
            if (Oldstd != null)
            {
                //Oldstd.Id = student.Id;
                Oldstd.Name = student.Name;
                Oldstd.Email = student.Email;
                Oldstd.Address = student.Address;
                Oldstd.Age = student.Age;
                Oldstd.Course = student.Course;
                _context.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            var emp = _context.Students.FirstOrDefault(x =>x.Id == id);
            if (emp != null)
            {
                _context.Remove(emp);
                _context.SaveChanges();
            }
        }
    }
}
