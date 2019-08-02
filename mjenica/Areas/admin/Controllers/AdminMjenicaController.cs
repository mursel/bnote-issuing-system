using mjenica.Areas.admin.Models;
using mjenica.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mjenica.Areas.admin.Controllers
{
    [RouteArea("admin")]
    public class AdminMjenicaController : ProtectedController
    {
        // GET: admin/AdminMjenica
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IzmjenaMjenice(int? id)
        {
            ModelStatus status = new ModelStatus();

            if (id == null)
            {
                status.JeGreska = true;
                status.Opis = "Izmjena mjenice nije moguca: id = null";
                return View("Greska", status);
            }

            var mjenica = MjenicaService.Instance.GetById(id);

            if (mjenica == null)
            {
                return HttpNotFound("Mjenica sa ID (" + id + ") ne postoji u bazi podataka!");
            }


            return View(mjenica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzmjenaMjenice(Mjenica mjenica)
        {
            if (ModelState.IsValid)
            {

                //Mjenica m = MjenicaService.Instance.GetBySN(mjenica.BrojMjenice);

                //if (m != null)
                //{
                //    TempData["status"] = "Mjenica sa brojem (" + m.BrojMjenice + ") vec postoji u bazi podataka!";
                //    return View(m);
                //}

                ModelStatus status = MjenicaService.Instance.Edit(mjenica);
                if (status.JeGreska)
                {
                    return View("Greska", status);
                }
            }
            return RedirectToAction("PregledMjenica", "AdminMjenica");
        }

        public ActionResult PregledMjenica()
        {
            var lista = MjenicaService.Instance.VratiSve();

            return View(lista);
        }
    }


}