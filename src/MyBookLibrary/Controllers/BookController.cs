using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBookLibrary.Data;
using MyBookLibrary.Models;
using Newtonsoft.Json.Linq;

namespace MyBookLibrary.Controllers
{
    public class Composite
    {
        public List<Book> books { get; set; }
        public String userId { get; set; }

    }

    [Authorize]
    public class BookController : Controller
    {
        BookContext model;
        private UserManager<ApplicationUser> _userManager;
        private String _userId;

        public BookController(BookContext model, UserManager<ApplicationUser> _userManager)
        {
            this.model = model;
            this._userManager = _userManager;
        }

        [Authorize]
        // GET: /<controller>/
        public IActionResult New()
        {
            this._userId = _userManager.GetUserId(HttpContext.User);
            ViewBag._userId = _userManager.GetUserId(HttpContext.User);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult New([FromBody]List<Book> books)
        {

            foreach (var book in books)
            {                
                model.Books.Add(book);                
            }

            model.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
