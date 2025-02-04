using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Controllers
{
    public class ServicesController : Controller
    {
        string routeName = "協會業務";

        // GET: Services  //Jobs//協會業務
        public ActionResult Index()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "協會介紹";

            return View();
        }

        // GET: Services/Licenses //訓練、認證、發照
        public ActionResult Licenses()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "訓練、認證、發照";

            return View();
        }

        // GET: Services/Refer //諮詢、顧問
        public ActionResult Refer()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "諮詢、顧問";

            return View();
        }

        // GET: Services/Survey //縱火調查
        public ActionResult Survey()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "縱火調查";

            return View();
        }

    }
}