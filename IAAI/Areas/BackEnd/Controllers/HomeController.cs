using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Areas.BackEnd.Controllers
{
    public class HomeController : Controller
    {
        // GET: BackEnd/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}