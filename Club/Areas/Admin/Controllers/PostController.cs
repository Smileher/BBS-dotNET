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
        public ActionResult Index() {
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
        }
        public ActionResult Status() {
            // 审核删帖
            return View();
        }
        public ActionResult Category() {
            // 分类管理
            return View();
        }


    }
}