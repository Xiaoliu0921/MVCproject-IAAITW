using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Expert:BaseEntity
    {

        [Display(Name = "照片")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "現職")]
        public string CurrentPosition { get; set; }

        
        [Display(Name = "學歷 ")]
        public string Education { get; set; }

        
        [Display(Name = "基本介紹")]
        public string Introduction { get; set; }

        
        [Display(Name = "經歷(簡述)")]
        public string Experience { get; set; }

        
        [Display(Name = "專長")]
        public string Expertise { get; set; }

        
        [Display(Name = "鑑定實務經驗")]
        public string AppraisalExperience { get; set; }


        
        [Display(Name = "學術著作")]
        public string AcademicPublications { get; set; }

        
        [Display(Name = "研究計畫")]
        public string ResearchProjects { get; set; }

        
        [Display(Name = "訓練或資格認證")]
        public string TrainingOrCertifications { get; set; }

        
        [Display(Name = "經歷(含起訖年)")]
        public string DetailedExperience { get; set; }

        
        [Display(Name = "兼任公部門")]
        public string ConcurrentPublicSector { get; set; }

        
        [Display(Name = "兼任法人機構")]
        public string ConcurrentLegalAgency { get; set; }


    }
}