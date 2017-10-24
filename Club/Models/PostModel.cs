using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Club.Models
{
    /// <summary>
    /// 主页帖子模型
    /// </summary>
    public class ListPostModel
    {
        //帖子id
        public int id { get; set; }
        //帖子标题
        public string title { get; set; }
        //用户ID
        public int userid { get; set; }
        //发帖用户
        public string username { get; set; }
        //用户头像
        public string image { get; set; }
        //发帖时间
        public DateTime time { get; set; }
        //访问量
        public int visit { get; set; }
        //回复量
        public int relpy { get; set; }
        //是否是精华贴
        public string essence { get; set; }           
    }    
}