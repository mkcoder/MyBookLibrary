using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                var databaseBook = model.Books.SingleOrDefault(b => b.Isbn == book.Isbn && b.UserId == book.UserId);
                if (databaseBook == null)
                    model.Books.Add(book);                
            }

            model.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await model.Books.SingleOrDefaultAsync(m => m.BookID == id);
            if (note == null)
            {
                return NotFound();
            }

            model.Books.Remove(note);
            await model.SaveChangesAsync();
            return RedirectToAction("index", "Home");
        }

    }
}
