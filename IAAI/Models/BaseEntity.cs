using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class BaseEntity
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty]
        public int Id { get; set; }

        [Display(Name = "是否刪除")]
        public bool IsDelete { get; set; } = false;

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "最後編輯者")]
        public string UpdatedBy { get; set; } = "Unknown";

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "最後編輯時間")]
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "創建者")]
        public string Creator { get; set; } = "Unknown"; 

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "創建時間")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;


    }
}