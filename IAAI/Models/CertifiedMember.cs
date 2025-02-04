using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class CertifiedMember : BaseEntity
    {
        [Display(Name = "照片")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Company")]
        public string Company { get; set; }
    }
}