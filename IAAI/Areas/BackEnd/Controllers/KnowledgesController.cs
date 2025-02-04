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
    public class KnowledgesController : Controller
    {
        private DbModel db = new DbModel();
        string title = "Knowledges"; //ViewBag.Title
        string description = "管理知識庫下載檔案"; //ViewBag.Description
        private const int DefaultPageSize = 5;

        // GET: BackEnd/Knowledges
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.Title = title;
            ViewBag.Description = description;

            var knowledges = db.Knowledge
                .Where(n => n.IsDelete == false)
                .OrderByDescending(n => n.CreatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(knowledges);
            //return View(db.Knowledge.T。oList());
        }

        // GET: BackEnd/Knowledges/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // GET: BackEnd/Knowledges/Create
        public ActionResult Create(int? page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;
            return View();
        }

        // POST: BackEnd/Knowledges/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FileName")] Knowledge knowledge, HttpPostedFileBase FilePath)
        {
            // 移除模型中自動生成的錯誤訊息
            ModelState.Remove("FilePath");

            //判斷有沒有上傳檔案
            if (FilePath != null)
            {
                //檢查檔案大小(上限10MB)
                if (FilePath.ContentLength > 10 * 1024 * 1024) // 10MB = 10 * 1024 * 1024 bytes
                {
                    ModelState.AddModelError("FilePath", "檔案大小不得超過 10MB。");
                }
                knowledge.FilePath = Utility.SaveUpFile(FilePath);
            }
            else
            {
                ModelState.AddModelError("FilePath", "請選擇檔案!");
            }

            if (ModelState.IsValid)
            {
                db.Knowledge.Add(knowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(knowledge);
        }

        // GET: BackEnd/Knowledges/Edit/5
        public ActionResult Edit(int? id, int? page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // POST: BackEnd/Knowledges/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileName,FilePath,IsDelete,Creator,CreatedTime")] Knowledge knowledge,int? page, HttpPostedFileBase UploadFile)
        {
            if (UploadFile != null)
            {
                ModelState.Remove("FilePath");
                //檢查檔案大小(上限10MB)
                if (UploadFile.ContentLength > 10 * 1024 * 1024) // 10MB = 10 * 1024 * 1024 bytes
                {
                    ModelState.AddModelError("FilePath", "檔案大小不得超過 10MB。");
                }
                knowledge.FilePath = Utility.SaveUpFile(UploadFile);
            }

            if (ModelState.IsValid)
            {
                db.Entry(knowledge).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                page = page.HasValue ? page.Value : 1;
                return Redirect(Url.Action("Index") + $"?page={page}");
            }
            return View(knowledge);
        }

        // GET: BackEnd/Knowledges/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // POST: BackEnd/Knowledges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int? page)
        {
            try
            {
                // 根據 id 查找要刪除的項目
                var knowledgeItem = db.Knowledge.Find(id);
                if (knowledgeItem == null)
                {
                    TempData["ErrorMessage"] = "刪除失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                // 刪除項目
                knowledgeItem.IsDelete = true;
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
