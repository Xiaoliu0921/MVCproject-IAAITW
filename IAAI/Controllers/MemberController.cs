using IAAI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcPaging;
using Ganss.Xss;
using System.Net;

namespace IAAI.Controllers
{
    public class MemberController : Controller
    {
        private DbModel db = new DbModel();
        string routeName = "會員專區";
        private const int DefaultPageSize = 5;

        // GET: Member  //討論區
        public ActionResult Index(int? page)
        {
            //判斷是否有登入
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Member");
            }

            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "討論區";

            var posts = db.ForumPosts
                .Where(m => m.IsDelete == false)
                .OrderByDescending(m => m.CreatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(posts);
        }

        // GET: Member/Edit    // 修改個人資料
        public ActionResult Edit()
        {
            //判斷是否有登入
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Member");
            }


            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "修改個人資料";

            string userDataJson = Session["UserData"].ToString();
            var userData = JsonConvert.DeserializeObject<dynamic>(userDataJson);
            int memberId = userData.Id;
            var member = db.Members.Find(memberId);
            return View(member);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Account,PasswordSalt,Password,Name,Gender,Birthday,MemberType,ApplyingMemberType,PublicContactPhoneNumber,PhoneNumber,ContactAddress,Email,IsInternationalMembership,IsInternationalMembership,MembershipCertificateUrl,CurrentCompany,Position,HighestEducation,ServiceUnit1,ServiceUnit1Position,ServiceUnit1StartYear,ServiceUnit1StartMonth,ServiceUnit1EndYear,ServiceUnit1EndMonth,ServiceUnit2,ServiceUnit2Position,ServiceUnit2StartYear,ServiceUnit2StartMonth,ServiceUnit2EndYear,ServiceUnit2EndMonth,ServiceUnit3,ServiceUnit3Position,ServiceUnit3StartYear,ServiceUnit3StartMonth,ServiceUnit3EndYear,ServiceUnit3EndMonth,TotalExperienceYears,TotalExperienceMonths,IsDelete,CreatedTime,Creator")] Member member, string newPassword, HttpPostedFileBase UploadMembershipCertificateUrl)
        {

            //判斷檔案上傳
            if (member.IsInternationalMembership)
            {
                if (member.MembershipCertificateUrl != null)
                {
                    if (UploadMembershipCertificateUrl != null)
                    {
                        //檢查檔案大小(上限10MB)
                        if (UploadMembershipCertificateUrl.ContentLength > 10 * 1024 * 1024) // 10MB = 10 * 1024 * 1024 bytes
                        {
                            ModelState.Remove("IsInternationalMembership");
                            ModelState.AddModelError("IsInternationalMembership", "檔案大小不得超過 10MB。");
                            ViewBag.Message = ViewBag.Message == null ? "上傳檔案大小不得超過 10MB。<br />" : ViewBag.Message + "上傳檔案大小不得超過 10MB。<br />";

                        }
                        member.MembershipCertificateUrl = Utility.SaveUpFile(UploadMembershipCertificateUrl);
                    }

                }
                else
                {
                    if (UploadMembershipCertificateUrl != null)
                    {
                        //檢查檔案大小(上限10MB)
                        if (UploadMembershipCertificateUrl.ContentLength > 10 * 1024 * 1024) // 10MB = 10 * 1024 * 1024 bytes
                        {
                            ModelState.Remove("IsInternationalMembership");
                            ModelState.AddModelError("IsInternationalMembership", "檔案大小不得超過 10MB。");
                            ViewBag.Message = ViewBag.Message == null ? "上傳檔案大小不得超過 10MB。<br />" : ViewBag.Message + "上傳檔案大小不得超過 10MB。<br />";

                        }
                        member.MembershipCertificateUrl = Utility.SaveUpFile(UploadMembershipCertificateUrl);
                    }
                    else
                    {
                        ViewBag.Message = ViewBag.Message == null ? "未上傳會員證影本<br />" : ViewBag.Message + "未上傳會員證影本<br />";
                        ModelState.Remove("IsInternationalMembership");
                        ModelState.AddModelError("IsInternationalMembership", "請選擇檔案!");
                    }
                }
            }


            if (ModelState.IsValid)
            {

                //修改密碼密碼
                if (!string.IsNullOrEmpty(newPassword))
                {
                    // 密碼加密
                    //產生密碼鹽
                    member.PasswordSalt = Utility.CreateSalt();
                    //混合密碼鹽產生密碼雜湊
                    member.Password = Utility.GenerateHashWithSalt(member.Password, member.PasswordSalt);
                }

                db.SaveChanges();
                TempData["AlertMessage"] = "修改成功。";
                return View(member);
            }
            return View(member);
        }


        // Get: Member/Logout //登出(無View頁)
        public ActionResult Logout()
        {
            Session["Login"] = null;
            Session["UserData"] = null;
            return RedirectToAction("Login", "Member");
        }

        // GET: Member/Create //發表文章
        public ActionResult Create(int? page)
        {
            //判斷是否有登入
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Member");
            }

            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "討論區";
            ViewBag.page = page;
            return View();
        }

        // POST: Member/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,MemberId")] ForumPost forumPost, int? page)
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "討論區";
            ViewBag.page = page;

            var sanitizer = new HtmlSanitizer();
            forumPost.Content = sanitizer.Sanitize(forumPost.Content);

            db.ForumPosts.Add(forumPost);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Member/CreateRe  //回覆文章
        public ActionResult CreateRe(int? forumPostId, int? page)
        {
            //判斷是否有登入
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Member");
            }

            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "討論區";
            ViewBag.page = page;

            if (forumPostId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = db.ForumPosts.Find(forumPostId);
            if (forumPost == null)
            {
                return HttpNotFound();
            }

            ViewBag.ForumPostTitle = forumPost.Title;
            return View();
        }

        // POST: Member/CreateRe
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRe([Bind(Include = "Id,Title,Content,ForumPostId,MemberId")] ForumReply forumReply, int? page)
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "討論區";
            ViewBag.page = page;

            var sanitizer = new HtmlSanitizer();
            forumReply.Content = sanitizer.Sanitize(forumReply.Content);

            db.ForumReplys.Add(forumReply);
            db.SaveChanges();
            return RedirectToAction("Details", "Member", new { id = forumReply.ForumPostId });

        }


        // GET: Member/Details  //討論區文章
        public ActionResult Details(int? id, int? page)
        {
            //判斷是否有登入
            if (Session["Login"] == null)
            {
                return RedirectToAction("Login", "Member");
            }

            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "討論區";
            ViewBag.page = page;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForumPost forumPost = db.ForumPosts.Find(id);
            if (forumPost == null)
            {
                return HttpNotFound();
            }
            return View(forumPost);
        }


        // GET: Member/Login //登入
        public ActionResult Login()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "會員登入";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(IAAI.Models.ViewModels.LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                //登入檢查
                Member member = ValidateUser(login.Account, login.Password);
                if (member != null)
                {

                    if (member.IsApproved == false)
                    {
                        TempData["AlertMessage"] = "帳號尚未審核通過，待管理員審核後方可登入。";
                        return View();
                    }

                    //登入成功
                    string userData = JsonConvert.SerializeObject(new
                    {
                        Role = "Member",  //角色 判斷是Member(前台)還是Admin(後台)
                        Id = member.Id,
                        Account = member.Account,
                        Name = member.Name,
                    });
                    Session["Login"] = true;
                    Session["UserData"] = userData;
                    return RedirectToAction("Index", "Member");
                }
                ViewBag.message = "登入失敗!!";
                return View();

            }
            ViewBag.message = "登入失敗!!";
            return View();
        }

        private Member ValidateUser(string userName, string password)
        {
            Member member = db.Members.FirstOrDefault(o => o.Account == userName);
            if (member == null)
            {
                return null;
            }
            string saltPassword = Utility.GenerateHashWithSalt(password, member.PasswordSalt);
            return saltPassword == member.Password ? member : null;
        }


        // GET: Member/Register //註冊
        public ActionResult Register()
        {
            ViewBag.RouteName = routeName;
            ViewBag.SubRouteName = "會員註冊";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Account,Password,Name,Gender,Birthday,ApplyingMemberType,PublicContactPhoneNumber,PhoneNumber,ContactAddress,Email,IsInternationalMembership,IsInternationalMembership,CurrentCompany,Position,HighestEducation,ServiceUnit1,ServiceUnit1Position,ServiceUnit1StartYear,ServiceUnit1StartMonth,ServiceUnit1EndYear,ServiceUnit1EndMonth,ServiceUnit2,ServiceUnit2Position,ServiceUnit2StartYear,ServiceUnit2StartMonth,ServiceUnit2EndYear,ServiceUnit2EndMonth,ServiceUnit3,ServiceUnit3Position,ServiceUnit3StartYear,ServiceUnit3StartMonth,ServiceUnit3EndYear,ServiceUnit3EndMonth,TotalExperienceYears,TotalExperienceMonths")] Member member, string ConfirmPassword, string Captcha, HttpPostedFileBase UploadMembershipCertificateUrl)
        {
            //確認帳號
            if (db.Members.Any(m => m.Account == member.Account))
            {
                ViewBag.Message = ViewBag.Message == null ? "此帳號已有人註冊<br />" : ViewBag.Message + "此帳號已有人註冊<br />";
                ModelState.Remove("Account");
                ModelState.AddModelError("Account", "此帳號已有人註冊。");
            }

            //確認密碼
            if (!string.IsNullOrEmpty(member.Password))
            {
                if (member.Password != ConfirmPassword)
                {
                    ViewBag.confirmPasswordMessage = "確認密碼與密碼不一致!!";
                    ViewBag.Message = ViewBag.Message == null ? "確認密碼與密碼不一致<br />" : ViewBag.Message + "確認密碼與密碼不一致<br />";
                    ModelState.Remove("Password");
                    ModelState.AddModelError("Password", "確認密碼與密碼不一致。");
                }
            }

            //驗證碼
            // 從 Session 取得驗證碼
            var sessionCaptcha = Session["CaptchaCode"] as string;
            Session["CaptchaCode"] = null; //清空驗證碼
            if (string.IsNullOrEmpty(sessionCaptcha) || Captcha != sessionCaptcha)
            {
                Captcha = string.Empty;
                ModelState.Remove("Captcha");
                ViewBag.CaptchaMessage = "驗證碼錯誤";
                ViewBag.Message = ViewBag.Message == null ? "驗證碼錯誤<br />" : ViewBag.Message + "驗證碼錯誤<br />";
                ModelState.AddModelError("Captcha", "驗證碼錯誤");
            }

            //檔案
            if (member.IsInternationalMembership)
            {
                if (UploadMembershipCertificateUrl != null)
                {
                    //檢查檔案大小(上限10MB)
                    if (UploadMembershipCertificateUrl.ContentLength > 10 * 1024 * 1024) // 10MB = 10 * 1024 * 1024 bytes
                    {
                        ModelState.Remove("IsInternationalMembership");
                        ModelState.AddModelError("IsInternationalMembership", "檔案大小不得超過 10MB。");
                        ViewBag.Message = ViewBag.Message == null ? "上傳檔案大小不得超過 10MB。<br />" : ViewBag.Message + "上傳檔案大小不得超過 10MB。<br />";

                    }
                    member.MembershipCertificateUrl = Utility.SaveUpFile(UploadMembershipCertificateUrl);
                }
                else
                {
                    ViewBag.Message = ViewBag.Message == null ? "未上傳會員證影本<br />" : ViewBag.Message + "未上傳會員證影本<br />";
                    ModelState.Remove("IsInternationalMembership");
                    ModelState.AddModelError("IsInternationalMembership", "請選擇檔案!");
                }
            }


            if (ModelState.IsValid)
            {


                // 密碼加密
                //產生密碼鹽
                member.PasswordSalt = Utility.CreateSalt();
                //混合密碼鹽產生密碼雜湊
                var newPassword = Utility.GenerateHashWithSalt(member.Password, member.PasswordSalt);
                member.Password = newPassword;

                db.Members.Add(member);
                db.SaveChanges();
                TempData["AlertMessage"] = "註冊成功！\n待管理員審核通過後方可登入。";
                return RedirectToAction("Login", "Member");
            }
            return View(member);
        }

    }
}