using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAAI.Models.ViewModels
{
    public class MemberSearchViewModel
    {
        public string OrderBy { get; set; } = "1";
        public string SearchAccount { get; set; } = string.Empty;
        public string SearchName { get; set; } = string.Empty;
        public string[] MemberType { get; set; }
        public DateTime? StartUpdateDate { get; set; }
        public DateTime? EndUpdateDate { get; set; }
        public DateTime? StartCreateDate { get; set; }
        public DateTime? EndCreateDate { get; set; }
    }
}