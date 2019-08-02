using mjenica.DAL;
using mjenica.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace mjenica.Areas.admin.Controllers
{
    [RouteArea("admin")]
    public class KorisniciController : ProtectedController
    {
        // GET: admin/Korisnici
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IzmjenaKorisnika(int? id)
        {
            ModelStatus status = new ModelStatus();

            if (id == null)
            {
                status.JeGreska = true;
                status.Opis = "Izmjena korisnika nije moguca: id = null";
                return View("Greska", status);
            }

            var korisnik = KorisnikService.Instance.GetById(id);

            if (korisnik == null)
            {
                return HttpNotFound("Korisnik sa ID (" + id + ") ne postoji u bazi podataka!");
            }
            

            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzmjenaKorisnika([Bind(Include ="Id,UserID,Username")] Korisnik korisnik)
        {
            if(ModelState.IsValid)
            {

                //Korisnik k = KorisnikService.Instance.GetByUserID(korisnik.UserID);

                //if (k != null)
                //{
                //    TempData["status"] = "Korisnik sa UserID (" + k.UserID + ") vec postoji u bazi podataka!";
                //    return View(korisnik);
                //}

                ModelStatus status = KorisnikService.Instance.Edit(korisnik);
                if (status.JeGreska)
                {
                    return View("Greska", status);
                }
            }
            return RedirectToAction("PregledKorisnika", "Korisnici");
        }

        public ActionResult NoviKorisnik()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NoviKorisnik(Korisnik korisnik)
        {
            if (!ModelState.IsValid)
                return View(korisnik);
            else
            {
                ModelStatus status = KorisnikService.Instance.AddNew(korisnik);

                if (status.JeGreska)
                {
                    return View("Greska", status);
                }

                TempData["ok"] = "Unos korisnika (" + korisnik.Username + ") uspješno izvršen!";
            }
            return View("Index");
        }
        
        public ActionResult PregledKorisnika()
        {
            var lista = KorisnikService.Instance.VratiSve();

            return View(lista);
        }
    }
}