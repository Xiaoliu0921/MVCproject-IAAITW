using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class ForumPost:BaseEntity
    {
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "文章標題")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "文章內容")]
        public string Content { get; set; }

        [Display(Name = "發文者")]
        public int MemberId { get; set; }

        [JsonIgnore]
        [ForeignKey("MemberId")]
        public virtual Member MyMember { get; set; }

        public virtual ICollection<ForumReply> MyReplies { get; set; }



    }
}