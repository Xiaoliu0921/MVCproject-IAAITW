using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAI.Models;
using IAAI.Models.ViewModels;
using MvcPaging;

namespace IAAI.Areas.BackEnd.Controllers
{
    [CustomAuthorize]
    public class MembersController : Controller
    {
        private DbModel db = new DbModel();
        string title = "Member"; //ViewBag.Title
        string description = "管理前台會員"; //ViewBag.Description
        private const int DefaultPageSize = 5;

        // GET: BackEnd/Members
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;

            ViewBag.Title = title;
            ViewBag.Description = description;

            if (Session["memberSearch"] != null)
            {
                var query = db.Members.AsQueryable();

                query = query.Where(n => n.IsDelete == false);

                string searchJson = Session["memberSearch"].ToString();
                MemberSearchViewModel search = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberSearchViewModel>(searchJson);

                //排序
                switch (search.OrderBy)
                {
                    case "1": // 待審核會員優先排序
                        query = query.OrderBy(m => string.IsNullOrEmpty(m.MemberType.ToString()) ? 0 : 1)
                            .ThenBy(m => m.MemberType)
                            .ThenByDescending(n => n.UpdatedTime);
                        break;
                    case "2": // 依會員類別降序排序
                        query = query.OrderByDescending(m => string.IsNullOrEmpty(m.MemberType.ToString()) ? "1" : m.MemberType.ToString())
                            .ThenByDescending(n => n.UpdatedTime);
                        break;
                    case "3": // 依會員類別升序排序
                        query = query.OrderBy(m => string.IsNullOrEmpty(m.MemberType.ToString()) ? "1" : m.MemberType.ToString())
                            .ThenByDescending(n => n.UpdatedTime);
                        break;
                    case "4": // 依創建時間越新越先
                        query = query.OrderByDescending(n => n.CreatedTime);
                        break;
                    case "5": // 依創建時間越晚越先
                        query = query.OrderBy(n => n.CreatedTime);
                        break;
                    case "6": // 依編輯時間越新越先
                        query = query.OrderByDescending(n => n.UpdatedTime);
                        break;
                    case "7": // 依編輯時間越晚越先
                        query = query.OrderBy(n => n.UpdatedTime);
                        break;
                    default:
                        query = query.OrderBy(m => string.IsNullOrEmpty(m.MemberType.ToString()) ? 0 : 1)
                            .ThenBy(m => m.MemberType)
                            .ThenByDescending(n => n.UpdatedTime);
                        break;
                }

                //帳戶搜尋
                if (!string.IsNullOrEmpty(search.SearchAccount))
                {
                    query = query.Where(n => n.Account.Contains(search.SearchAccount));
                }

                //名稱搜尋
                if (!string.IsNullOrEmpty(search.SearchName))
                {
                    query = query.Where(n => n.Name.Contains(search.SearchName));
                }

                //會員類型搜尋
                if (search.MemberType != null && search.MemberType.Length > 0)
                {
                    if (search.MemberType.Contains("待審核會員"))
                    {
                        query = query.Where(n => string.IsNullOrEmpty(n.MemberType.ToString())|| search.MemberType.Contains(n.MemberType.ToString()));

                    }
                    else
                    {
                        query = query.Where(n => search.MemberType.Contains(n.MemberType.ToString()));
                    }
                }

                //編輯日期區間
                if (search.StartUpdateDate.HasValue)
                {
                    query = query.Where(n =>
                        DbFunctions.TruncateTime(n.UpdatedTime) >= DbFunctions.TruncateTime(search.StartUpdateDate));
                }

                if (search.EndUpdateDate.HasValue)
                {
                    query = query.Where(n =>
                        DbFunctions.TruncateTime(n.UpdatedTime) <= DbFunctions.TruncateTime(search.EndUpdateDate));
                }

                //創建日期區間
                if (search.StartCreateDate.HasValue)
                {
                    query = query.Where(n =>
                        DbFunctions.TruncateTime(n.CreatedTime) >= DbFunctions.TruncateTime(search.StartCreateDate));
                }

                if (search.EndCreateDate.HasValue)
                {
                    query = query.Where(n =>
                        DbFunctions.TruncateTime(n.CreatedTime) <= DbFunctions.TruncateTime(search.EndCreateDate));
                }

                var searchedNews = query.ToPagedList(page.Value, DefaultPageSize);
                return View(searchedNews);

            }

            var members = db.Members
                .Where(m => m.IsDelete == false)
                .OrderBy(m => string.IsNullOrEmpty(m.MemberType.ToString()) ? 0 : 1) // 將 null 或空白值視為 1，其他視為 0
                .ThenBy(m => m.MemberType) // 對非 null 的值進行進一步排序
                .ThenByDescending(m => m.UpdatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(members);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MemberSearchViewModel search)
        {
            Session["memberSearch"] = Newtonsoft.Json.JsonConvert.SerializeObject(search);

            return RedirectToAction("Index");
        }

        // GET: LinkCategories/Details/5
        public ActionResult Details(int? id, int? page)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            ViewBag.page = page;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approval(int? id, int? page)
        {
            try
            {
                // 根據 id 查找項目
                var memberItem = db.Members.Find(id);
                if (memberItem == null)
                {
                    TempData["ErrorMessage"] = "找不到該資料，審核失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                memberItem.MemberType = memberItem.ApplyingMemberType;
                memberItem.ApplyingMemberType = null;
                memberItem.IsApproved = true;
                db.SaveChanges();

                return RedirectToAction("Index", new { page = page });
            }
            catch (Exception ex)
            {
                // 記錄錯誤信息
                System.Diagnostics.Debug.WriteLine("審核失敗: " + ex.Message);
                //TempData["ErrorMessage"] = "刪除失敗: " + ex.Message;
                TempData["ErrorMessage"] = "審核失敗。";
                return RedirectToAction("Index", new { page = page });
            }
        }

        // GET: BackEnd/Members/Edit/5
        public ActionResult Edit(int? id, int? page)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            ViewBag.page = page;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: BackEnd/Members/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Account,Password,PasswordSalt,IsApproved,Name,Gender,Birthday,MemberType,ApplyingMemberType,PublicContactPhoneNumber,PhoneNumber,ContactAddress,Email,IsInternationalMembership,MembershipCertificateUrl,CurrentCompany,Position,HighestEducation,ServiceUnit1,ServiceUnit1Position,ServiceUnit1StartYear,ServiceUnit1StartMonth,ServiceUnit1EndYear,ServiceUnit1EndMonth,ServiceUnit2,ServiceUnit2Position,ServiceUnit2StartYear,ServiceUnit2StartMonth,ServiceUnit2EndYear,ServiceUnit2EndMonth,ServiceUnit3,ServiceUnit3Position,ServiceUnit3StartYear,ServiceUnit3StartMonth,ServiceUnit3EndYear,ServiceUnit3EndMonth,TotalExperienceYears,TotalExperienceMonths,Creator,CreatedTime")] Member member, int? page, string newPassword)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(newPassword) == false)
                {
                    // 密碼加密
                    //產生密碼鹽
                    member.PasswordSalt = Utility.CreateSalt();
                    //混合密碼鹽產生密碼雜湊
                    var password = Utility.GenerateHashWithSalt(newPassword, member.PasswordSalt);
                    member.Password = password;
                }


                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                page = page.HasValue ? page.Value : 1;
                return Redirect(Url.Action("Index") + $"?page={page}");
            }
            return View(member);
        }


        // POST: BackEnd/Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int? page)
        {
            try
            {
                // 根據 id 查找要刪除的項目
                var memberItem = db.Members.Find(id);
                if (memberItem == null)
                {
                    TempData["ErrorMessage"] = "刪除失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                // 刪除項目
                memberItem.IsDelete = true;
                db.SaveChanges();

                return RedirectToAction("Index", new { page = page });
            }
            catch (Exception ex)
            {
                // 記錄錯誤信息
                System.Diagnostics.Debug.WriteLine("刪除失敗: " + ex.Message);
                //TempData["ErrorMessage"] = "刪除失敗: " + ex.Message;
                TempData["ErrorMessage"] = "刪除失敗。";
                return RedirectToAction("Index", new { page = page });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
