using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Club;
using Club.Models;
using static Club.AuthFilter;

namespace Club.Controllers
{
    public class PostController : BaseController
    {        
        /// <summary>
        /// 帖子列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Post
        public ActionResult Index(int?id)
        {
            var pageIndex = id ?? 1;
            int pageSize = 10;
            //自定义 
            //      key==1 为搜索
            //      key==2 为帖子最新发布和回复选择
            //      key==3 为帖子类型选择
            var key = Request["key"].ToInt();
            var value = Request["value"];
            var listpost=new List<ListPostModel>();
            using (var db = new Entities())
            {
                var type = db.Category.ToList();
                ViewBag.type = type;
                var post = db.Post.OrderByDescending(a => a.Id).Include(a => a.User).Include(a => a.Category).Where(a => a.IsFeatured == true).ToList();
                //按帖子类型查找
                switch(key)
                {
                    case 1:
                        post = post.Where(a => a.Title.Contains(value) || a.User.Name.Contains(value)).ToList();
                        break;
                    case 2:
                        break;
                    case 3:
                        var typeid = value.ToInt();
                        //typeid==0  全部帖子
                        if(typeid!=0)
                        {
                            post = post.Where(a => a.CategoryId == typeid).ToList();
                        }                        
                        break;
                }                
                foreach (var item in post)
                {
                    var postModel = new ListPostModel();
                    var reply = db.Reply.Where(a => a.Id == item.Id);
                    postModel.Id = item.Id;
                    postModel.Title = item.Title;
                    postModel.Userid = item.UserId;
                    postModel.Username = item.User.Name;
                    postModel.Image = item.User.Image;
                    postModel.Time = item.CreateTime;
                    postModel.Visit = item.ViewCount;
                    postModel.Relpy = reply.Count();
                    if(item.IsFeatured == true)
                    {
                        postModel.Essence = "精";
                    }
                    listpost.Add(postModel);
                }
                listpost = listpost.ToPagedList(pageIndex: pageIndex, pageSize: pageSize);
                //判断是否为ajax请求
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Post", listpost);
                }
            }
            return View(listpost);
        }
        /// <summary>
        /// 帖子搜索(暂时不用)
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }
        /// <summary>
        /// 帖子发布
        /// </summary>
        /// <returns></returns>
        [UserCheck(IsNeed = true)]
        public ActionResult New()
        {
            using (var db=new Entities())
            {
                var type = db.Category.ToList();
                ViewBag.type = type;
            }
            return View();
        }
        [UserCheck(IsNeed = true)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Save()
        {
            var typeId = Request["typeId"].ToInt();
            var title = Request["title"];
            var content = Request["content"];
            var loginUser = (User)Session["loginuser"];
            using (var db=new Entities())
            {
                var post = new Post {
                    Title = title,
                    Content = content,
                    UserId = loginUser.Id,
                    CategoryId = typeId,
                    Sysinfo = "Win10",
                    CreateTime = DateTime.Now
                };
                db.Post.Add(post);
                db.SaveChanges();
            }            
            return Redirect("/");
        }
        /// <summary>
        /// 帖子浏览
        /// </summary>
        /// <returns></returns>
        public ActionResult Browse()
        {
            var postid = Request["postid"].ToInt();
            if(postid!=0)
            {
                using (var db=new Entities())
                {
                    var post = db.Post.Include(a=>a.User).Include(a => a.Category).FirstOrDefault(a => a.Id == postid);
                    var user = db.User.OrderByDescending(a => a.Id).Include(a => a.Level).ToList();                    
                    ViewBag.User = user;
                    //查询赞帖子的用户
                    var praiserecord = db.PraiseRecord.OrderByDescending(a => a.Id).Include(a => a.User).ToList();
                    ViewBag.praiserecord = praiserecord;
                    var listpraiserecord = new List<PraiserecordModel>();
                    foreach (var item in praiserecord)
                    {
                        var praiserecordmodel = new PraiserecordModel {
                            postid = item.PostId,
                            userid = item.UserId,
                            username = item.User.Name,
                            userimage = item.User.Image,
                            time = item.CreateTime
                        };
                        listpraiserecord.Add(praiserecordmodel);
                    }
                    ViewData["praiserecord"] = listpraiserecord;
                    //查询收藏帖子的用户
                    var collection = db.Collection.OrderByDescending(a => a.Id).Include(a => a.User).ToList();                    
                    ViewBag.collection = collection;
                    //查询帖子回复的信息
                    var reply = db.Reply.OrderByDescending(a => a.Id).Include(a => a.User).Where(a=>a.PostId == postid).ToList();                    
                    var listreply = new List<ReplyModel>();
                    foreach(var item in reply)
                    {
                        var replyModel = new ReplyModel();
                        replyModel.postid = item.PostId;
                        replyModel.userid = item.UserId;
                        replyModel.username = item.User.Name;
                        replyModel.userlevel = item.User.Level.Name;
                        replyModel.userimage = item.User.Image;
                        replyModel.content = item.Content;
                        replyModel.recoverytime = item.CreateTime;
                        listreply.Add(replyModel);
                    }
                    ViewData["reply"] = listreply;                    
                    return View(post);                    
                }                
            }
            return View();
        }
        /// <summary>
        /// 帖子回复
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]//允许传html代码格式的值
        public ActionResult Replysave()
        {
            int postid = Request["postid"].ToInt();
            int userid = Request["userid"].ToInt();
            string content = Request["content"].ToString();
            using (var db=new Entities())
            {
                var reply = new Reply();
                reply.PostId = postid;
                reply.UserId = userid;
                reply.Content = content;
                reply.CreateTime = DateTime.Now;
                db.Reply.Add(reply);
                db.SaveChanges();
                //查询帖子回复的信息
                var listreply = db.Reply.OrderByDescending(a => a.Id).Include(a => a.User).Where(a => a.PostId == postid).ToList();
                var replyModel = new ReplyModel();
                var listreplymodel = new List<ReplyModel>();
                foreach (var item in listreply)
                {
                    replyModel.postid = item.PostId;
                    replyModel.userid = item.UserId;
                    replyModel.username = item.User.Name;
                    replyModel.userlevel = item.User.Level.Name;
                    replyModel.userimage = item.User.Image;
                    replyModel.content = item.Content;
                    replyModel.recoverytime = item.CreateTime;
                    listreplymodel.Add(replyModel);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Comment", listreplymodel);
                }
            }
            return View();
        }
        /// <summary>
        /// 用户赞贴
        /// </summary>
        /// <returns></returns>
        public ActionResult PraiseCollect()
        {
            var key= Request["key"].ToInt();
            var postid = Request["postid"].ToInt();
            var userid = Request["userid"].ToInt();            
            using (var db = new Entities())
            {
                if(key==1)
                {
                    var praiserecord = new PraiseRecord();
                    praiserecord.PostId = postid;
                    praiserecord.UserId = userid;
                    praiserecord.CreateTime = DateTime.Now;
                    db.PraiseRecord.Add(praiserecord);
                    db.SaveChanges();
                }
                else
                {
                    var collection = new Collection();
                    collection.PostId = postid;
                    collection.UserId = userid;
                    collection.CreateTime = DateTime.Now;
                    db.Collection.Add(collection);
                    db.SaveChanges();
                }
                //查询赞帖子的用户
                var praiserecords = db.PraiseRecord.OrderByDescending(a => a.Id).Include(a => a.User).ToList();
                ViewBag.praiserecord = praiserecords;
                var listpraiserecord = new List<PraiserecordModel>();
                foreach (var item in praiserecords)
                {
                    var praiserecordmodel = new PraiserecordModel();
                    praiserecordmodel.postid = item.PostId;
                    praiserecordmodel.userid = item.UserId;
                    praiserecordmodel.username = item.User.Name;
                    praiserecordmodel.userimage = item.User.Image;
                    praiserecordmodel.time = item.CreateTime;
                    listpraiserecord.Add(praiserecordmodel);
                }                
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_PraiseRecord", listpraiserecord);
                }                    
            }
            return RedirectToAction("Browse");
        }        
    }
}