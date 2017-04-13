using Microsoft.EntityFrameworkCore;
using MyBookLibrary.Models;

namespace MyBookLibrary.Data
{
    public sealed class BookContext : DbContext
    {
        public BookContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
