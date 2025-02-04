using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IAAI.Models
{
    public class News : BaseEntity
    {
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "標題")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "{0}必填")]
        [Display(Name = "預覽圖")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "文章內容")]
        //[DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "是否置頂")]
        public bool IsTop { get; set; } = false;

        [Display(Name = "瀏覽次數")]
        public int ViewCount { get; set; } = 0;

        [NotMapped]
        [Display(Name = "簡短內容")]
        public string ShortContent
        {
            get
            {
                // 移除 HTML 標籤
                string plainText = Regex.Replace(Content ?? "", "<.*?>", string.Empty);

                // 處理 HTML 實體，例如 &nbsp; 轉換成空白
                plainText = System.Net.WebUtility.HtmlDecode(plainText);
                // 只取前 50 個字
                return plainText.Length > 50 ? plainText.Substring(0, 50) + "..." : plainText;
            }
        }


    }
}