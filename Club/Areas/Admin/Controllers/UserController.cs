using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Club.Areas.Admin.Controllers
{
    public class UserController:Controller
    {
        // GET: Admin/User
        public ActionResult Index() {
            List<User> list;
            using(var db = new Entities()) {
                list = db.User.Include("Level").ToList();
            }
            return View(list);
        }
        public ActionResult Admin() {
           
            return View();
        }
        public ActionResult Details() {
            return View();
        }
    }
}