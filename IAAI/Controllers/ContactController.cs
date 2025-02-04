using IAAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Controllers
{
    public class ContactController : Controller
    {
        string routeName = "聯絡我們";

        // GET: Contact
        public ActionResult Index()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "聯絡我們";

            return View();
        }

        [HttpPost]
        public ActionResult Index(IAAI.Models.ViewModels.ContactViewModel contact)
        {



            if (ModelState.IsValid)
            {

                bool success = true;

                // 從 Session 取得驗證碼
                var sessionCaptcha = Session["CaptchaCode"] as string;
                Session["CaptchaCode"] = null; //清空驗證碼
                if (string.IsNullOrEmpty(sessionCaptcha) || contact.Captcha != sessionCaptcha)
                {
                    contact.Captcha = string.Empty;
                    ModelState.AddModelError("Captcha", "驗證碼錯誤，請再試一次！");
                    success = false;
                }

                if (Utility.IsValidEmail(contact.Email) == false)
                {
                    ModelState.AddModelError("Email", "Email 格式錯誤！");
                    success = false;
                }

                if (!success)
                {
                    return View(contact);
                }

                StringBuilder sb = new StringBuilder();
                sb.Append(contact.Name + " 先生/小姐您好：<br />");
                sb.Append("<br />");
                sb.Append("感謝您的來信，我們將盡快回覆您！<br />");
                sb.Append("以下是您的詢問表單：<br />");
                sb.Append("姓名：" + contact.Name + "<br />");
                sb.Append("性別：" + contact.Gender.ToString() + "<br />");
                sb.Append("連絡電話：" + contact.PhoneNumber + "<br />");
                sb.Append("E-Mail：" + contact.Email + "<br />");
                sb.Append("詢問內容：" + contact.Content + "<br />");
                sb.Append("<br />");
                sb.Append("<br />");
                sb.Append("本郵件是由系統自動寄發，請勿直接回覆，請靜候我們的回覆，謝謝！");

                Utility.SendGmail("國際縱火調查人員協會臺灣分會", "rocket2024@gmail.com", contact.Name, contact.Email, "我們已收到您的發問-國際縱火調查人員協會臺灣分會網站", sb.ToString());

                TempData["Message"] = "感謝您的填寫，我們將盡快回覆您！";
                return RedirectToAction("Index");
            }
            return View(contact);
        }
    }
}