using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
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