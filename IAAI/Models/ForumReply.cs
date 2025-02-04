using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class ForumReply:BaseEntity
    {
        

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "回覆內容")]
        public string Content { get; set; }

        [Display(Name = "回覆之文章")]
        public int ForumPostId { get; set; }

        [JsonIgnore]
        [ForeignKey("ForumPostId")]
        public virtual ForumPost MyForumPost { get; set; }

        [Display(Name = "發文者")]
        public int MemberId { get; set; }

        [JsonIgnore]
        [ForeignKey("MemberId")]
        public virtual Member MyMember { get; set; }

    }
}