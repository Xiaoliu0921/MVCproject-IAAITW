using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Admin:BaseEntity
    {
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "密碼鹽")]
        public string PasswordSalt { get; set; }

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

        //[Required(ErrorMessage = "{0}必填")]
        [Display(Name = "現職單位")]
        public string CurrentCompany { get; set; }

        //[Required(ErrorMessage = "{0}必填")]
        [Display(Name = "職稱")]
        public string Position { get; set; }

        //[Required(ErrorMessage = "{0}必填")]
        [Display(Name = "權限")]
        public string Permission { get; set; }


    }
}