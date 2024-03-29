﻿using Microsoft.EntityFrameworkCore;
using MyBookLibrary.Models;

namespace MyBookLibrary.Data
{
    public sealed class BookContext : DbContext
    {
        public BookContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Note> Notes { get; set; }
    }
}
