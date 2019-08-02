using mjenica.DAL;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace mjenica.Controllers
{
    public class MjenicaController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NovaMjenica()
        {
            
            //FileStream fs = new FileStream(Server.MapPath(@"~\Sabloni\template1.rtf"), FileMode.Open, FileAccess.Read);
            //StreamReader sr = new StreamReader(fs);
            //string sve = sr.ReadToEnd();
            //sr.Close();
            //fs.Close();
            
            Document doc = new Document(Server.MapPath(@"~\Sabloni\template1.doc"));

            var sifra = KorisnikService.Instance.VratiSifruKorisnika(User.Identity.Name);
            doc.Replace("#s#", sifra, true, true);

            var zadnjiSerBroj = MjenicaService.Instance.VratiZadnjiSerijskiBroj();

            int br = int.Parse(zadnjiSerBroj);
            br++;

            var finalSerBroj = br.ToString("D8");

            doc.Replace("#serbroj#",finalSerBroj, true, true);
            doc.SaveToFile(Server.MapPath(@"~\Sabloni\template1_.pdf"), FileFormat.PDF);

            MemoryStream ms = new MemoryStream();
            doc.SaveToStream(ms, FileFormat.PDF);

            string mjenicaName = "mjenica-" + DateTime.Now.ToString().Trim().Replace(".", "").Replace("-", "").Replace("/", "").Replace(":", "").Trim() + ".pdf";

            FileContentResult fcr = new FileContentResult(ms.ToArray(), "application/pdf")
            {
                FileDownloadName = mjenicaName
            };

            ms.Close();

            var result = MjenicaService.Instance.DodajMjenicu(sifra, finalSerBroj);
            if (result.JeGreska)
                return View("Greska", result);

            return fcr;
        }
    }
}