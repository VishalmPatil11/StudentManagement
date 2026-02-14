using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Service
{
    public interface IStudent
    {
        IQueryable<Student> GetStudents();
        Student GetStudent(int id);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}
