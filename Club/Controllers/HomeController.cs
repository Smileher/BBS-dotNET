using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Club;

namespace Club.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            using (var club = new ClubEntitie())
            {
                //添加
                //var level = new Level();
                //level.Name = "帮主";
                //club.Level.Add(level);
                //club.SaveChanges();

                //删除
                //var level = club.Level.FirstOrDefault(a => a.id == 3);
                //if(level!=null)
                //{
                //    club.Level.Remove(level);
                //    club.SaveChanges();
                //}

                //查找
                //var level = club.Level.Where(a => a.id == 1).ToList();
                //if(level!=null)
                //{
                //    var str = new StringBuilder();
                //    foreach (var item in level)
                //    {
                //        str.AppendLine("id:" + item.id + " name:" + item.Name);                        
                //    }
                //    return Content(str.ToString());
                //}

            }
            return Content("ok");
        }

        public ActionResult About()
        {
            //using (var club=new ClubEntitie())
            //{
            //    string psw = "000000";
            //    var user = club.User.ToList();
            //    foreach(var item in user)
            //    {
            //        item.Password = psw.MD5Encoding(item.Account);
            //    }
            //    club.SaveChanges();
            //}

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}