using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using System.Diagnostics;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
