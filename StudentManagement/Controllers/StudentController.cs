using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Service;

namespace StudentManagement.Controllers
{
    public class StudentController : Controller
    {
        public readonly IStudent _student;

        public StudentController(IStudent student)
        {
            _student = student;
        }

        public IActionResult Index()
        {
            var stds = _student.GetStudents();
            return View(stds);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var std = _student.GetStudent(id);
            return View(std);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _student.AddStudent(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var std = _student.GetStudent(id);
            if (std == null)
            {
                return View("Error");
            }
            return View(std);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _student.UpdateStudent(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var std = _student.GetStudent(id);
            if (std == null)
            {
                return View("Error");
            }
            return View(std);
        }
        [HttpPost]
        public IActionResult Delete(Student student)
        {
            if (ModelState.IsValid)
            {
                _student.DeleteStudent(student.Id);
                return RedirectToAction("Index");
            }
            return View(student);
        }
        [HttpGet]
        public IActionResult Search(string searchString)
        {
            IQueryable<Student> query = _student.GetStudents();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string term = searchString.Trim().ToLower();

                int num;
                bool isNumber = int.TryParse(term, out num);

                query = query.Where(p => p.Name.ToLower().Contains(term));

                if(!query.Any() && isNumber)
                {
                    query = _student.GetStudents().Where(p => p.Age == num);
                }
            }
            var result = query.ToList();
            return View(result);
        }
    }
}
