using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Club.Areas.Admin.Controllers
{
    [AuthFilter(IsNeedLogin = false)]
    public class LoginController:BaseController
    {
        // GET: Admin/Login
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Login() {
            var account = Request["account"];
            var password = Request["password"];


            if(string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password)) {
                ShowMassage("账号密码不能为空");
                return RedirectToAction("Index");
            }

            using(var db = new Entities()) {

                var pw = password.MD5Encoding(account);

                var user = db.User.Where(a => a.Account == account && a.PassWord == pw);
                if(user == null) {
                    ShowMassage("用户不存在");
                    return RedirectToAction("Index");
                }
                //设置用户登陆状态
                Session["loginUser"] = user;
                return Redirect("/admin/home");
            }
        }
        [HttpPost]
        public ActionResult Register() {
            var name = Request["reg_name"];
            var account = Request["reg_account"];
            var password = Request["reg_password"];

            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                ShowMassage("用户名，账号或密码不能为空");

            using(var db = new Entities()) {
                var db_account = db.User.FirstOrDefault(a => a.Account == account);

                try {
                    if(account == db_account.ToString()) {
                        ShowMassage("用户名重复");
                    }
                }
                catch {

                }
                var user = new User {
                    Account = account.ToString(),
                    PassWord = password.MD5Encoding(account)
                };
                db.User.Add(user);
                db.SaveChanges();
                ShowMassage("注册成功");
                return RedirectToAction("Index");
            }
        }
    }
}