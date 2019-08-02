using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mjenica.Areas.admin.Models
{
    public class ModelStatus
    {
        [Required]
        [Display(Name = "JeGreska")]
        public bool JeGreska { get; set; }

        [Required]
        [Display(Name ="Naziv greske")]
        public string Opis { get; set; }
    }
}