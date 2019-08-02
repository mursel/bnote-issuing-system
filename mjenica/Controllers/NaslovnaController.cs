using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mjenica.Controllers
{
    public class NaslovnaController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}