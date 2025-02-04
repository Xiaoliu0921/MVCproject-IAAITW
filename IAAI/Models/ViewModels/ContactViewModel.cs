using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "性別")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "聯絡電話")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "詢問內容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "驗證碼")]
        public string Captcha { get; set; }

    }
}