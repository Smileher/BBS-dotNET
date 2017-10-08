using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Club.Controllers
{
    public class HomeController:Controller
    {
        public ActionResult Index() {
            using(var club=new Entities()) {
                var user=club.User.ToList();
                foreach(var item in user) {
                    item.PassWord = item.PassWord.MD5Encoding(item.Account);
                }
                club.SaveChanges();
            }
                return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}