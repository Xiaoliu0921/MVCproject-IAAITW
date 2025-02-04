using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Knowledge:BaseEntity
    {
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name ="檔案名稱")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "檔案路徑")]
        public string FilePath { get; set; }


    }
}