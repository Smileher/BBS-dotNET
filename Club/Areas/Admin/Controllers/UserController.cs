using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace Club.Areas.Admin.Controllers
{
    public class UserController:Controller
    {
        // GET: Admin/User
        /// <summary>
        /// 分页操作 MVCpager
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            int pageSize = 1;
            var indexStr = Request["pageIndex"];
            if(string.IsNullOrEmpty(indexStr)) {
                indexStr = "1";
            }
            var pageIndex = int.Parse(indexStr);

            var list=new List<User>();
            using(var db = new Entities()) {
                list = db.User.Where(a => a.IsAbort == false).OrderByDescending(a => a.Id).Include("Level").ToPagedList(pageIndex: pageIndex,pageSize:pageSize);
                return View(list);
            }
        }
        public ActionResult Admin() {

            return View();
        }
        public ActionResult Details() {
            return View();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns>返回列表</returns>
        public ActionResult Delete() {
            //获取Id
            var idStr = Request["Id"];
            //判断Id是否为空，如果为空则返回"输入错误"
            if(string.IsNullOrEmpty(idStr)) {
                return Content("输入错误");
            }
            //将string类型的Id转换为int类型
            var id = int.Parse(idStr);
            //
            using(var db = new Entities()) {
                var user = db.User.FirstOrDefault(a => a.Id == id);
                //判断是否为空
                if(User != null) {
                    //逻辑删除，将IsAbort赋值为ture;
                    user.IsAbort = true;
                    //直接删除user
                    //db.User.Remove(user);
                    //保存
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}