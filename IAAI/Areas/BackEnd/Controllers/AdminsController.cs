using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAI.Models;
using Microsoft.Ajax.Utilities;
using MvcPaging;

namespace IAAI.Areas.BackEnd.Controllers
{
    [CustomAuthorize]
    public class AdminsController : Controller
    {
        private DbModel db = new DbModel();
        string title = "Admins"; //ViewBag.Title
        string description = "管理後台管理員帳號"; //ViewBag.Description
        private const int DefaultPageSize = 5;

        // GET: BackEnd/Admins
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.Title = title;
            ViewBag.Description = description;

            var admins = db.Admins
                .Where(n => n.IsDelete == false)
                .OrderByDescending(n => n.CreatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(admins);
            //return View(db.Admins.ToList());
        }

        // GET: BackEnd/Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BackEnd/Admins/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Account,Password,Name,Gender,PhoneNumber,Email,CurrentCompany,Position")] Admin admin)
        {
            //確認帳號
            if (db.Admins.Any(m => m.Account == admin.Account))
            {
                ModelState.Remove("Account");
                ModelState.AddModelError("Account", "此帳號已有人註冊。");
            }



            if (ModelState.IsValid)
            {
                // 密碼加密
                //產生密碼鹽
                admin.PasswordSalt = Utility.CreateSalt();
                //混合密碼鹽產生密碼雜湊
                var newPassword = Utility.GenerateHashWithSalt(admin.Password, admin.PasswordSalt);
                admin.Password = newPassword;

                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: BackEnd/Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: BackEnd/Admins/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Account,Password,PasswordSalt,Name,Gender,PhoneNumber,Email,CurrentCompany,Position,Permission,IsDelete,UpdatedBy,UpdatedTime,Creator,CreatedTime")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: BackEnd/Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: BackEnd/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int? page)
        {
            try
            {
                // 根據 id 查找要刪除的項目
                var adminItem = db.Admins.Find(id);
                if (adminItem == null)
                {
                    TempData["ErrorMessage"] = "刪除失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                // 刪除項目
                adminItem.IsDelete = true;
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
