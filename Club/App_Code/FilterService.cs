﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Club
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,Inherited = true,AllowMultiple = false)]
    public class AuthFilter:ActionFilterAttribute
    {

        public bool IsNeedLogin { get; set; } = false;

        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            if(!IsNeedLogin) {
                base.OnActionExecuting(filterContext);
                return;
            }

            var loginUser = filterContext.HttpContext.Session["loginUser"];

            if(loginUser != null) {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new RedirectResult("/admin/login");
            return;

        }



        //var loginUser = filterContext.HttpContext.Session["loginUser"];

        //filterContext.HttpContext.Response.Write("我是执行前打出来的" + Name);
        //    }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            base.OnActionExecuted(filterContext);
            //filterContext.HttpContext.Response.Write("我是执行后打出来的" + Name);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext) {
            base.OnResultExecuting(filterContext);
            //filterContext.HttpContext.Response.Write("我是在结果执行前打出来的" + Name);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext) {
            base.OnResultExecuted(filterContext);
            // filterContext.HttpContext.Response.Write("我是结果执行后打出来的" + Name);
        }
    }

}