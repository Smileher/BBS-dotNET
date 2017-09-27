using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Webdiyer.WebControls.Mvc;

namespace Club.Areas.Admin.Controllers
{
    public class UserController:Controller
    {
        // GET: Admin/User
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns>返回list</returns>
        public ActionResult Index() {
            //分页操作 MVCpager
            int pageSize = 15;
            var indexStr = Request["pageIndex"];
            if(string.IsNullOrEmpty(indexStr)) {
                indexStr = "1";
            }
            var pageIndex = int.Parse(indexStr);

            var list = new List<User>();
            using(var db = new Entities()) {
                list = db.User.Where(a => a.IsAbort == false).OrderByDescending(a => a.Id).Include("Level").ToPagedList(pageIndex: pageIndex,pageSize: pageSize);
                return View(list);
            }
            //随机生成1000个测试用户
            //using(var db = new Entities()) {
            //    for(int i = 0;i < 1000;i++) {
            //        var user = new User();
            //        var random = new Random();
            //        //随机产生范围为1-4的数字并赋值给LevelId
            //        user.LevelId = random.Next(1,4);
            //        //测试用户的Name为"测试用户+i"
            //        user.Name = "测试用户" + i;
            //        //获取一个五位随机数转换成String类型,并赋值给Account和PassWord
            //        user.Account = random.Next(10000,99999).ToString();
            //        user.PassWord = random.Next(10000,99999).ToString();
            //        //添加
            //        db.User.Add(user);
            //        //保存
            //        db.SaveChanges();
            //    }
            //    return Content("生成1000个测试用户成功!");
            //}

        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns>返回列表</returns>
        public ActionResult Delete() {
            //获取Id
            var idStr = Request["Id"];
            var id = idStr.AsInt();
            //判断Id是否为0，如果为0则提示"数据不正确！"
            if(id == 0) {
                TempData["Msg"] = "数据不正确！";
                return RedirectToAction("Index");
            }
            //将string类型的Id转换为int类型
            //var id = int.Parse(idStr);
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
            TempData["Msg"] = "删除成功！";
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin() {
            return View();
        }
        /// <summary>
        /// 用户收藏点赞列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Details() {
            return View();
        }
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit() {
            return View();
        }
    }
}