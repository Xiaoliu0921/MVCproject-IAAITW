using IAAI.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace IAAI.Areas.BackEnd.Controllers
{
    public class LoginController : Controller
    {
        private DbModel db = new DbModel();


        // GET: BackEnd/Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: BackEnd/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IAAI.Models.ViewModels.LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                //登入檢查
                Admin admin = ValidateUser(login.Account, login.Password);
                if (admin != null)
                {
                    //登入成功
                    string userData = JsonConvert.SerializeObject(new
                    {
                        Role="Admin",  //角色 判斷是Member(前台)還是Admin(後台)
                        Id = admin.Id,
                        Account = admin.Account,
                        Name = admin.Name,
                    });
                    Utility.SetAuthenTicket(userData, login.Account);
                    return RedirectToAction("Index", "News");
                }
                ViewBag.message = "登入失敗!!";
                return View();

            }
                ViewBag.message = "登入失敗!!";
            return View();
        }

        private Admin ValidateUser(string userName, string password)
        {
            Admin admin = db.Admins.FirstOrDefault(o => o.Account == userName);
            if (admin == null)
            {
                return null;
            }
            string saltPassword = Utility.GenerateHashWithSalt(password, admin.PasswordSalt);
            return saltPassword == admin.Password ? admin : null;
        }

        // Get: Member/Logout //登出(無View頁)
        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}