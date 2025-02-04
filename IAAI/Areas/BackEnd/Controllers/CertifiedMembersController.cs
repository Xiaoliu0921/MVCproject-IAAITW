using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using IAAI.Models;
using MvcPaging;

namespace IAAI.Areas.BackEnd.Controllers
{
    [CustomAuthorize]
    public class CertifiedMembersController : Controller
    {
        private DbModel db = new DbModel();
        string title = "CertifiedMembers"; //ViewBag.Title
        string description = "管理配證會員資料"; //ViewBag.Description
        private const int DefaultPageSize = 5;

        // GET: BackEnd/CertifiedMembers
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.Title = title;
            ViewBag.Description = description;

            var certifiedMembers = db.CertifiedMembers
                .Where(n => n.IsDelete == false)
                .OrderByDescending(n => n.CreatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(certifiedMembers);
            //return View(db.CertifiedMembers.ToList());
        }

        // GET: BackEnd/CertifiedMembers/Create
        public ActionResult Create(int? page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;

            return View();
        }

        // POST: BackEnd/CertifiedMembers/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Country,Position,Company")] CertifiedMember certifiedMember,HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {

                //判斷有沒有上傳預覽圖
                if (UploadImage != null)
                {
                    //判斷是不是圖檔
                    if (UploadImage.ContentType.IndexOf("image", System.StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        return View(certifiedMember);
                    }
                    certifiedMember.ImageUrl = Utility.SaveUpImage(UploadImage);
                }
                else
                {
                    //未上傳圖片就給預設圖
                    certifiedMember.ImageUrl = $"/UploadFiles/Images/profile.jpg";
                }



                db.CertifiedMembers.Add(certifiedMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(certifiedMember);
        }

        // GET: BackEnd/CertifiedMembers/Edit/5
        public ActionResult Edit(int? id,int?page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertifiedMember certifiedMember = db.CertifiedMembers.Find(id);
            if (certifiedMember == null)
            {
                return HttpNotFound();
            }
            return View(certifiedMember);
        }

        // POST: BackEnd/CertifiedMembers/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,FirstName,LastName,Country,Position,Company,IsDelete,Creator,CreatedTime")] CertifiedMember certifiedMember,int? page,HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid)
            {
                //判斷有沒有上傳預覽圖
                if (UploadImage != null)
                {
                    //判斷是不是圖檔
                    if (UploadImage.ContentType.IndexOf("image", System.StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        ViewBag.Message = "檔案型態錯誤!";
                        return View(certifiedMember);
                    }
                    certifiedMember.ImageUrl = Utility.SaveUpImage(UploadImage);
                }

                db.Entry(certifiedMember).State = EntityState.Modified;
                db.SaveChanges();
                page = page.HasValue ? page.Value : 1;
                return Redirect(Url.Action("Index")+$"?page={page}");
            }
            return View(certifiedMember);
        }


        // POST: BackEnd/CertifiedMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int? page)
        {
            try
            {
                // 根據 id 查找要刪除的項目
                var certifiedMemberItem = db.CertifiedMembers.Find(id);
                if (certifiedMemberItem == null)
                {
                    TempData["ErrorMessage"] = "刪除失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                // 刪除項目
                certifiedMemberItem.IsDelete = true;
                db.SaveChanges();

                return RedirectToAction("Index", new { page = page });
            }
            catch (Exception ex)
            {
                // 記錄錯誤信息
                System.Diagnostics.Debug.WriteLine("刪除失敗: " + ex.Message);
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
