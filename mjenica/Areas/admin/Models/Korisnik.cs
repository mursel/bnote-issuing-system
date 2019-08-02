using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace mjenica.Areas.admin.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Sifra")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Korisnik")]
        public string Username { get; set; }



    }
}