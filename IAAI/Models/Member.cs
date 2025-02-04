using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Member : BaseEntity
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

        [Display(Name = "是否審核通過")]
        public bool IsApproved { get; set; }=false;

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "性別")]
        public GenderType Gender { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime2")] // 指定資料庫中的型別為 datetime2
        public DateTime Birthday { get; set; }

        [Display(Name = "會員類別")]
        public MemberType? MemberType { get; set; }

        [Display(Name = "申請中會員類別")]
        public MemberType? ApplyingMemberType { get; set; }

        [Display(Name = "聯絡電話(公)")]
        public string PublicContactPhoneNumber { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "聯絡電話")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "通訊地址")]
        public string ContactAddress { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "國際會籍")]
        public bool IsInternationalMembership { get; set; } = false;


        [Display(Name = "國際會籍會員證影本")]
        public string MembershipCertificateUrl { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "現職單位")]
        public string CurrentCompany { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "職稱")]
        public string Position { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "最高學歷")]
        public string HighestEducation { get; set; }

        [Display(Name = "服務經歷_服務單位1")]
        public string ServiceUnit1 { get; set; }

        [Display(Name = "服務經歷_服務單位1職稱")]
        public string ServiceUnit1Position { get; set; }

        [Display(Name = "服務經歷_服務單位1開始年")]
        public string ServiceUnit1StartYear { get; set; }

        [Display(Name = "服務經歷_服務單位1開始月")]
        public string ServiceUnit1StartMonth { get; set; }

        [Display(Name = "服務經歷_服務單位1結束年")]
        public string ServiceUnit1EndYear { get; set; }

        [Display(Name = "服務經歷_服務單位1結束月")]
        public string ServiceUnit1EndMonth { get; set; }


        [Display(Name = "服務經歷_服務單位2")]
        public string ServiceUnit2 { get; set; }

        [Display(Name = "服務經歷_服務單位2職稱")]
        public string ServiceUnit2Position { get; set; }

        [Display(Name = "服務經歷_服務單位2開始年")]
        public string ServiceUnit2StartYear { get; set; }

        [Display(Name = "服務經歷_服務單位2開始月")]
        public string ServiceUnit2StartMonth { get; set; }

        [Display(Name = "服務經歷_服務單位2結束年")]
        public string ServiceUnit2EndYear { get; set; }

        [Display(Name = "服務經歷_服務單位2結束月")]
        public string ServiceUnit2EndMonth { get; set; }

        [Display(Name = "服務經歷_服務單位3")]
        public string ServiceUnit3 { get; set; }

        [Display(Name = "服務經歷_服務單位3職稱")]
        public string ServiceUnit3Position { get; set; }

        [Display(Name = "服務經歷_服務單位3開始年")]
        public string ServiceUnit3StartYear { get; set; }

        [Display(Name = "服務經歷_服務單位3開始月")]
        public string ServiceUnit3StartMonth { get; set; }

        [Display(Name = "服務經歷_服務單位3結束年")]
        public string ServiceUnit3EndYear { get; set; }

        [Display(Name = "服務經歷_服務單位3結束月")]
        public string ServiceUnit3EndMonth { get; set; }

        [Display(Name = "相關年資合計(年)")]
        public int TotalExperienceYears { get; set; }

        [Display(Name = "相關年資合計(月)")]
        public int TotalExperienceMonths { get; set; }


        public virtual ICollection<ForumPost> MyPosts { get; set; }

        public virtual ICollection<ForumReply> MyReplys { get; set; }

    }
}