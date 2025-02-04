using IAAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Controllers
{
    public class AboutController : Controller
    {
        DbModel db = new DbModel();

        string routeName = "關於我們";

        // GET: About //協會介紹頁
        public ActionResult Index()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "協會介紹";
            return View();
        }

        // GET: About/Organization //組織架構頁
        public ActionResult Organization()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "組織架構";
            return View();
        }

        // GET: About/History //沿革頁
        public ActionResult History()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "沿革";
            return View();
        }

        // GET: About/Member //配證會員頁
        public ActionResult Member()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "配證會員";
            var certifiedMembers = db.CertifiedMembers.Where(cm=>cm.IsDelete==false).ToList();

            return View(certifiedMembers);
        }

        // GET: About/Expert //專家介紹頁
        public ActionResult Expert()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "專家介紹";
            var experts = db.Experts.Where(e => e.IsDelete == false).ToList();
            return View(experts);
        }

        // GET: About/Expert //專家介紹頁
        public ActionResult ExpertDetails(int? id)
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "專家介紹";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

    }
}