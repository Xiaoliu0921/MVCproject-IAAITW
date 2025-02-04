using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAAI.Models;

namespace IAAI.Controllers
{
    public class HomeController : Controller
    {
        DbModel db = new DbModel();

        // GET: Home
        public ActionResult Index()
        {

            var news=db.News.Where(n=>n.IsDelete==false)
                .OrderByDescending(n => n.IsTop)
                .ThenByDescending(n=>n.UpdatedTime)
                .Take(4)
                .ToList();

            return View(news);
        }
    }
}