using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mjenica.Areas.admin.Models
{
    public class Mjenica
    {
        [Required]
        [Display(Name ="Id")]
        public int MjenicaId { get; set; }

        [Display(Name = "Šifra korisnika")]
        public string SifraKorisnika { get; set; }

        [Display(Name = "Datum izdavanja")]
        public DateTime Datum { get; set; }

        [Display(Name = "Broj mjenice")]
        public string BrojMjenice { get; set; }

        [Display(Name ="Validna mjenica?"), DisplayFormat(ApplyFormatInEditMode =true, ConvertEmptyStringToNull = true, NullDisplayText = "0")]
        public int JeValidna { get; set; }

    }
}