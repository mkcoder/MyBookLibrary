using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyBookLibrary.Models
{
    /* 
     * Description: This model is used to store user notes about a book and their current progress
     * BookNotes
     */
    public class Note
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]        
        public int NoteID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }
        public string Isbn { get; set; }
        [Display(Name = "Page Number")]
        public int PageNumber { get; set; }
        [Display(Name = "Notes")]
        public String PageNote { get; set; }      
    }
}
