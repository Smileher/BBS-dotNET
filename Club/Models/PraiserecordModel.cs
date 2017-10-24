using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Club.Models
{
    public class PraiserecordModel
    {                
        //所赞帖子的id
        public int postid { get; set; }
        //赞贴用户的id
        public int userid { get; set; }
        //赞贴用户名
        public string username { get; set; }
        //赞贴用户头像
        public string userimage { get; set; }
        //赞贴时间
        public DateTime time { get; set; }
    }
}