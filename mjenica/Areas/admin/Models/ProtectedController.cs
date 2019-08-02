using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mjenica.Areas.admin.Models
{
    [Authorize(Roles = "Administrators")]
    public abstract class ProtectedController : Controller
    {
    }
}