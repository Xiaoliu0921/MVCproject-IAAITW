using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAAI.Models.ViewModels
{
    public class NewsSearchViewModel
    {
        public string OrderBy { get; set; } = "1";
        public string SearchTitle { get; set; }= string.Empty;
        public string SearchContent { get; set; } = string.Empty;
        public DateTime? StartUpdateDate { get; set; }
        public DateTime? EndUpdateDate { get; set; }
        public DateTime? StartCreateDate { get; set; }
        public DateTime? EndCreateDate { get; set; }
    }
}