using mjenica.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mjenica.Areas.admin.Controllers
{
    [RouteArea("admin")]
    public class AdminBackupController : ProtectedController
    {
        // GET: admin/AdminBackup
        public ActionResult Index()
        {
            return View();
        }
    }
}