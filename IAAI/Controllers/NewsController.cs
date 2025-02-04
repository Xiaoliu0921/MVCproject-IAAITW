using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using IAAI.Models;
using MvcPaging;

namespace IAAI.Controllers
{
    public class NewsController : Controller
    {
        private DbModel db = new DbModel();
        string routeName = "訊息發布";
        private const int DefaultPageSize = 5;


        // GET: News
        public ActionResult Index(int? page)
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "最新消息";

            page = page.HasValue ? page.Value - 1 : 0;

            var news = db.News
                .Where(n => n.IsDelete == false)
                .OrderByDescending(n => n.IsTop)
                .ThenByDescending(n => n.UpdatedTime)
                .ToPagedList(page.Value, DefaultPageSize);



            return View(news);
        }


        // GET: News/Details
        public ActionResult Details(int Id)
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "訊息發布";

            var news = db.News.FirstOrDefault(n => n.Id == Id && n.IsDelete == false);
            if (news == null)
            {
                TempData["ErrorMessage"] = "找不到該筆資料";
                return RedirectToAction("Index","News");
            }

            return View(news);
        }
    }
}