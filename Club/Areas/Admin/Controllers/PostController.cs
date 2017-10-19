using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace Club.Areas.Admin.Controllers
{
    public class PostController:BaseController
    {
        /// <summary>
        /// 帖子管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // 帖子列表
            //分页操作 MVCpager
            int pageSize = 15;
            var indexStr = Request["pageIndex"];
            if(string.IsNullOrEmpty(indexStr)) {
                indexStr = "1";
            }
            var pageIndex = int.Parse(indexStr);
            //查询
            PagedList<Post> items;
            var kw = Request["kw"];
            using(var db = new Entities()) {
                var list = db.Post.Where(a => a.IsAbort == false).Include(a => a.User).Include(a => a.Category);

                if(!string.IsNullOrEmpty(kw)) {
                    list = list.Where(a => a.User.Name.Contains(kw) || a.Title.Contains(kw));
                    ViewBag.KW = kw;
                }

                items = list.OrderByDescending(a => a.Id).ToPagedList(pageIndex: pageIndex,pageSize: pageSize);
            }
            return View(items);
            //随机生成300个帖子
            //using (var db = new Entities())
            //{
            //    for (int i = 0; i < 300; i++)
            //    {
            //        var post = new Post();
            //        var random = new Random();

            //        post.Title = "第" + i + "个帖子";
            //        post.UserId = 1026;
            //        post.CreateTime = Convert.ToDateTime("2017-10-10 00:00:00.000");
            //        post.Sysinfo = "windows10";
            //        post.Content = "内容" + i;
            //        post.ViewCount = random.Next(1, 9999);
            //        post.ReplyCount = random.Next(1, 9999);
            //        post.PostPointer = 0;
            //        post.ParentId = 1;
            //        post.CategoryId = random.Next(1, 3);
            //        post.IsFeatured = Convert.ToBoolean("False");
            //        post.Status = Convert.ToBoolean("True");
            //        post.IsAbort = Convert.ToBoolean("False");
            //        //添加
            //        db.Post.Add(post);
            //        //保存
            //        db.SaveChanges();
            //    }
            //    return Content("生成300个帖子成功!");
            //}
        }
        [HttpGet]
        public ActionResult Edit() {
            var Id = Request["Id"].ToInt();
            ViewData["title"] = "编辑帖子";
            string[] sg = { "未审核","已审核"};
            using(var db = new Entities()) {
                var post = db.Post.Include(a => a.User).FirstOrDefault(a => a.Id == Id);


                var selectItems = new List<SelectListItem>();

                var levels = db.Level.ToList();

                for(int i=0;i<2;i++) {
                    var selectItem = new SelectListItem {
                        Text = sg[i],
                        Value = i.ToString()
                    };
                    if(post != null && (post.Status == Convert.ToBoolean(i))) {
                        selectItem.Selected = true;
                    }
                    selectItems.Add(selectItem);
                }                
                ViewBag.SeletItems = selectItems;
                return View(post);
            }

        }
        [HttpPost]
        public ActionResult Save() {
            var id = Request["id"].ToInt();
            int status = Request["status"].ToInt();


            using(var db = new Entities()) {
                var post = db.Post.FirstOrDefault(a => a.Id == id);

                post.Status = Convert.ToBoolean(status);
                db.SaveChanges();
                ShowMassage("操作成功");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete() {
            //获取Id
            var id = Request["Id"].ToInt();
            //判断Id是否为0，如果为0则提示"数据不正确！"
            if(id == 0) {
                TempData["Msg"] = "数据不正确！";
                return RedirectToAction("Index");
            }
            //将string类型的Id转换为int类型
            //var id = int.Parse(idStr);
            using(var db = new Entities()) {
                var post = db.Post.FirstOrDefault(a => a.Id == id);
                //判断是否为空
                if(User != null) {
                    //逻辑删除，将IsAbort赋值为ture;
                    post.IsAbort = true;
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
        /// 帖子分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Category() {
            // 分类管理
            using(var db = new Entities()) {
                var category = db.Category.ToList();
                
                return View(category);
            }
        }
        /// <summary>
        /// 编辑帖子分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditCategory() {
            var Id = Request["Id"].ToInt();
            //var name = Request["Name"];
            ViewData["title"] = "编辑分类";
            using(var db = new Entities()) {
                var category = db.Category.FirstOrDefault(a => a.Id == Id);
                if(Id==0) {
                    category = new Category();
                    ViewData["title"] = "新增分类";                  
                }
                return View(category);
            }
        }
        /// <summary>
        /// 保存帖子分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCategory() {
            var id = Request["id"].ToInt();
            var name = Request["name"];
            using(var db = new Entities()) {
                var category = db.Category.FirstOrDefault(a => a.Id == id);
                
                if(category == null) {
                    category = new Category();
                    db.Category.Add(category);

                }
                category.Name = name;
                db.SaveChanges();
                ShowMassage("操作成功");
            }
            return RedirectToAction("Category");
        }
        /// <summary>
        /// 删除帖子分类
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteCategory() {
            //获取Id
            var id = Request["Id"].ToInt();
            //判断Id是否为0，如果为0则提示"数据不正确！"
            if(id == 0) {
                TempData["Msg"] = "数据不正确！";
                return RedirectToAction("Category");
            }
            //将string类型的Id转换为int类型
            //var id = int.Parse(idStr);
            using(var db = new Entities()) {
                var category = db.Category.FirstOrDefault(a => a.Id == id);
                //判断是否为空
                if(category != null) {
                    //直接删除
                    db.Category.Remove(category);
                    //保存
                    db.SaveChanges();
                }
            }
            TempData["Msg"] = "删除成功！";
            return RedirectToAction("Category");
        }


    }
}