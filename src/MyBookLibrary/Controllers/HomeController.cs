using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookLibrary.Data;
using MyBookLibrary.Models;

namespace MyBookLibrary.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly BookContext _repository;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(BookContext repository, UserManager<ApplicationUser> _userManager)
        {
            this._repository = repository;
            this._userManager = _userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var result = _repository.Books.Where(b => b.UserId == userId).ToList();

            return View(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
