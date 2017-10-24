using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Club.Models
{
    public class ReplyModel
    {
        //帖子id
        public int postid { get; set; }
        //用户ID
        public int userid { get; set; }
        //用户名
        public string username { get; set; }
        //用身份
        public string userlevel { get; set; }
        //用头像
        public string userimage { get; set; }
        //回复内容
        public String content { get; set; }
        //回复时间
        public DateTime recoverytime { get; set; }
    }
}