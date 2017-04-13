using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyBookLibrary.Models
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]        
        public int BookID { get; set; }
        public String Isbn { get; set; }
        public String Author { get; set; }
        public bool HaveRead { get; set; }
        public String UserId { get; set; }
        public String Image { get; set; }
    }
}
