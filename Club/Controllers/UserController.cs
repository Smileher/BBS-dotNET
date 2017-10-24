using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Club.Models;
using System.Data.Entity;

namespace Club.Controllers
{
    public class UserController : BaseController
    {
        /// <summary>
        /// 用户页面
        /// </summary>
        /// <returns></returns>
        // GET: Login
        public ActionResult Index()
        {
            var userid = Request["userid"] ?? "";
            int id = userid.ToInt();
            if(id!=0)
            {
                using (var db=new ClubEntitie())
                {
                    var user = db.User.OrderByDescending(a => a.id).Include(a => a.Level).FirstOrDefault(a => a.id == id);
                    Session["browseuser"] = user;
                }
            }
            return View();
        }
        public ActionResult Dynamic()
        {
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin UserLogin)
        {
            if (string.IsNullOrEmpty(UserLogin.Account) || string.IsNullOrEmpty(UserLogin.Password))
            {
                TempData["login"] = "用户名或密码不能为空";
                return RedirectToAction("Login");
            }
            using (var club = new ClubEntitie())
            {
                UserLogin.Password = UserLogin.Password.MD5Encoding(UserLogin.Account);
                var user = club.User.OrderByDescending(a => a.id).Include(a =>a.Level).FirstOrDefault(a => a.Account == UserLogin.Account);
                if (user == null)
                {
                    TempData["login"] = "用户名不存在";
                    return RedirectToAction("Login");
                }
                if (user.Password != UserLogin.Password)
                {
                    TempData["login"] = "密码错误";
                    TempData["account"] = UserLogin.Account;
                    return RedirectToAction("Login");
                }
                Session["loginuser"] = user;
                ViewBag.username = UserLogin.Account;
            }
            return Redirect("/Post");
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserRegister UserRegister)
        {
            using (var db=new ClubEntitie())
            {
                var user = db.User.ToList();
                foreach(var item in user)
                {
                    if(item.Account==UserRegister.Account)
                    {
                        ShowMassage("账号已存在");
                        return View();
                    }
                }
                var useradd = new User();
                useradd.Account = UserRegister.Account;
                useradd.Name = UserRegister.Name;
                useradd.Password = UserRegister.Password.MD5Encoding(UserRegister.Account);
                useradd.Levelid = 1;
                useradd.RegistrationTime = DateTime.Now;
                db.User.Add(useradd);
                db.SaveChanges();
                ShowMassage("注册成功，请登录");
            }
            return View();
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        public ActionResult Introduction()
        {
            Session["loginuser"] = null;
            return Redirect("/Post");
        }
    }
}