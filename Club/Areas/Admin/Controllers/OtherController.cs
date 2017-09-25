using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Club.Areas.Admin.Controllers
{
    public class OtherController : Controller
    {
        // GET: Admin/Other
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Developer() {
            return View();
        }
        public ActionResult Data() {
            return View();
        }
    }
}