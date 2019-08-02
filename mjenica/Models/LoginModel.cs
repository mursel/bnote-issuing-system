using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mjenica.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Korisničko ime: ")]
        public string KorisnickoIme { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka: ")]
        public string Lozinka { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool ZapamtiMe { get { return true; } }  // uvjek pamti 

    }
}