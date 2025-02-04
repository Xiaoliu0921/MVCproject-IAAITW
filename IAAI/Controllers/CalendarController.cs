using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Controllers
{
    public class CalendarController : Controller
    {
        string routeName = "行事曆";

        // GET: Calendar //協會行事曆
        public ActionResult Index()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "協會行事曆";

            return View();
        }
    }
}