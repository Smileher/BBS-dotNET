using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Webdiyer.WebControls.Mvc;
using Club;
using System.Threading;

namespace Club.Areas.Admin.Controllers
{
    public class UserController:BaseController
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
            //查询
            PagedList<User> items;
            var kw = Request["kw"];
            using(var db = new Entities()) {
                var list = db.User.Where(a => a.IsAbort == false).OrderByDescending(a=>a.Id).Include(a => a.Level);

                if(!string.IsNullOrEmpty(kw)) {
                    list = list.Where(a => a.Account.Contains(kw) || a.Name.Contains(kw));
                    ViewBag.KW = kw;
                }

                items = list.OrderByDescending(a => a.Id).ToPagedList(pageIndex: pageIndex,pageSize: pageSize);
            }
            return View(items);

           

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
            //随机生成100个测试用户
            //using(var db = new Entities()) {
            //    for(int i = 0;i < 100;i++) {
            //        var user = new User();
            //        var random = new Random();
            //        //随机产生范围为1-4的数字并赋值给LevelId
            //        user.LevelId = random.Next(1,5);
            //        //测试用户的Name为"测试用户+i"
            //        user.Name = "测试用户" + i;
            //        //获取一个五位随机数转换成String类型,并赋值给Account
            //        user.Account = random.Next(10000,99999).ToString();
            //        var account = user.Account;
            //        user.PassWord = 000000.ToString().MD5Encoding(account);
            //        头像
            //        user.Image = "/Assets/avatars/avatars (" + random.Next(1,32).ToString() + ").png";
            //        随机产生注册日期
            //        var x= random.Next(1996,2018).ToString();
            //        var y = random.Next(1,13).ToString();
            //        var z = random.Next(1,32).ToString();
            //        user.CreateTime = Convert.ToDateTime(x + "-" + y + "-" + z);
            //        //添加
            //        db.User.Add(user);
            //        //保存
            //        db.SaveChanges();
            //        延迟500毫秒
            //        Thread.Sleep(500);
            //    }
            //    return Content("生成100个测试用户成功!");
            //}
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
        [HttpGet]
        public ActionResult Edit() {
            var Id = Request["Id"].ToInt();
            ViewData["title"] = "编辑用户";
            using(var db = new Entities()) {
                var user = db.User.Include(a => a.Level).FirstOrDefault(a => a.Id == Id);


                var selectItems = new List<SelectListItem>();

                var levels = db.Level.ToList();

                foreach(var level in levels) {
                    var selectItem = new SelectListItem {
                        Text = level.Name,
                        Value = level.Id.ToString()
                    };
                    if(user != null && (user.LevelId == level.Id)) {
                        selectItem.Selected = true;
                    }
                    selectItems.Add(selectItem);
                }

                ViewBag.SeletItems = selectItems;

                if(user == null) {
                    user = new User();
                    ViewData["title"] = "新增用户";
                }


                return View(user);
            }

        }

        [HttpPost]
        public ActionResult Save() {
            var id = Request["id"].ToInt();
            var name = Request["name"];
            var account = Request["account"];
            var password = Request["password"];
            var levelId = Request["levelId"].ToInt();
            var integral = Request["integral"].ToInt();
            var image = Request["image"];


            using(var db = new Entities()) {
                var user = db.User.FirstOrDefault(a => a.Id == id);

                if(id == 0) {
                    user = new User {
                        Account = account,
                        Name = name,
                        IsAdmin = false
                    };
                    db.User.Add(user);
                }

                user.Name = name;
                user.LevelId = levelId;
                user.Integral = integral;
                if(user.PassWord!=password) {
                    user.PassWord = password.MD5Encoding(account);
                }
                user.Image = image;
                db.SaveChanges();
                ShowMassage("操作成功");
            }

            //if (id == 0)
            //{
            //    using (var db = new ClubEntities())
            //    {
            //        var user = new User();
            //        user.Name = name;
            //        user.Account = account;
            //        user.LevelId = levelId;
            //        user.integral = integral;
            //        user.Image = image;
            //        user.IsAdmin = false;
            //        user.PassWord = "000000";

            //        db.User.Add(user);

            //        db.SaveChanges();

            //        ShowMassage("新增成功");
            //    }
            //}
            return RedirectToAction("Index");
        }

    }
}