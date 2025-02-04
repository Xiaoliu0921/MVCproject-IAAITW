using IAAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;

namespace IAAI.Controllers
{
    public class KnowledgeController : Controller
    {
        DbModel db = new DbModel();
        private const int DefaultPageSize = 5;

        string routeName = "知識庫";

        // GET: Knowledge //知識庫下載
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "知識庫下載";

            var knowledges = db.Knowledge.Where(k => k.IsDelete == false)
                .OrderByDescending(k => k.CreatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(knowledges);
        }
    }
}