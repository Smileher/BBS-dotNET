using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Club.Models;
using System.Data.Entity;
using Webdiyer.WebControls.Mvc;
using Microsoft.Ajax.Utilities;
using static Club.AuthFilter;

namespace Club.Controllers
{
    public class UserController:BaseController
    {
        static int key = 1;
        /// <summary>
        /// 用户页面
        /// </summary>
        /// <returns></returns>
        // GET: Login
        [UserCheck(IsNeed = false)]
        public ActionResult Index(int? id) {
            int userid = Request["userid"].ToInt();
            var user = (User)Session["loginuser"];
            int requestkey = Request["key"].ToInt();
            if(requestkey != 0) {
                key = requestkey;
            }
            var pageIndex = id ?? 1;
            int pageSize = 10;
            PagedList<Post> pageListpost;
            PagedList<Collection> pageListcollection;
            PagedList<Reply> pageListreply;
            using(var db = new Entities()) {
                //判断用户访问自己的信息还是访问其他用户的信息
                if(userid <= 0) {
                    if(user != null) {
                        userid = user.Id;
                    }
                    else {
                        ShowMassage("未知错误");
                        return Redirect("/Post/Index");
                    }
                }
                else {
                    user = db.User.Include(a => a.Level).FirstOrDefault(a => a.Id == userid);
                }
                ViewBag.User = user;
                //帖子数量
                var post = db.Post.OrderByDescending(a => a.Id).Where(a => a.UserId == userid);
                ViewBag.postnumber = post.Count();
                switch(key) {
                    //帖子
                    case 1:
                        ViewBag.url = "_MyPost";
                        pageListpost = post.ToPagedList(pageIndex: pageIndex,pageSize: pageSize);
                        if(Request.IsAjaxRequest()) {
                            return PartialView("_MyPost",pageListpost);
                        }
                        return View(pageListpost);
                    //回复
                    case 2:
                        ViewBag.url = "_MyReply";
                        var listreply = new List<ReplyModel>();
                        var reply = db.Reply.OrderByDescending(a => a.Id).Include(a => a.Post).Include(a => a.User).Where(a => a.UserId == userid).DistinctBy(a => a.PostId);
                        pageListreply = reply.ToPagedList(pageIndex: pageIndex,pageSize: pageSize);
                        if(Request.IsAjaxRequest()) {
                            return PartialView("_MyReply",pageListreply);
                        }
                        break;
                    //收藏
                    case 3:
                        ViewBag.url = "_MyDynamic";
                        var collection = db.Collection.OrderByDescending(a => a.Id).Include(a => a.Post).Where(a => a.UserId == userid);
                        pageListcollection = collection.ToPagedList(pageIndex: pageIndex,pageSize: pageSize);
                        if(Request.IsAjaxRequest()) {
                            return PartialView("_MyDynamic",pageListcollection);
                        }
                        break;
                    //粉丝
                    case 4:
                        break;
                }
            }
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserLogin UserLogin) {
            if(string.IsNullOrEmpty(UserLogin.Account) || string.IsNullOrEmpty(UserLogin.Password)) {
                ShowMassage("用户名或密码不能为空");
                return RedirectToAction("Login");
            }
            using(var club = new Entities()) {
                UserLogin.Password = UserLogin.Password.MD5Encoding(UserLogin.Account);
                var user = club.User.OrderByDescending(a => a.Id).Include(a => a.Level).FirstOrDefault(a => a.Account == UserLogin.Account);
                if(user == null) {
                    ShowMassage("用户名不存在");
                    return RedirectToAction("Login");
                }
                if(user.PassWord != UserLogin.Password) {
                    ShowMassage("密码错误");
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
        public ActionResult Register() {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserRegister UserRegister) {
            using(var db = new Entities()) {
                var user = db.User.ToList();
                foreach(var item in user) {
                    if(item.Account == UserRegister.Account) {
                        ShowMassage("账号已存在");
                        return View();
                    }
                }
                var useradd = new User();
                useradd.Account = UserRegister.Account;
                useradd.Name = UserRegister.Name;
                useradd.PassWord = UserRegister.Password.MD5Encoding(UserRegister.Account);
                useradd.LevelId = 1;
                useradd.CreateTime = DateTime.Now;
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
        public ActionResult Introduction() {
            Session["loginuser"] = null;
            return Redirect("/Post");
        }
    }
}