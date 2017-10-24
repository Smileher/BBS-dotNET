using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Club.Models
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserRegister
    {
        //用户账号
        public string Account { get; set; }
        //用户名
        public string Name { get; set; }
        //用户密码
        public string Password { get; set; }

    }
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLogin
    {
        //用户账号
        public string Account { get; set; }        
        //用户密码
        public string Password { get; set; }

    }
}