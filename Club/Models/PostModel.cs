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
        public int Id { get; set; }
        //帖子标题
        public string Title { get; set; }
        //用户ID
        public int Userid { get; set; }
        //发帖用户
        public string Username { get; set; }
        //用户头像
        public string Image { get; set; }
        //发帖时间
        public DateTime Time { get; set; }
        //访问量
        public int Visit { get; set; }
        //回复量
        public int Relpy { get; set; }
        //是否是精华贴
        public string Essence { get; set; }           
    }    
}