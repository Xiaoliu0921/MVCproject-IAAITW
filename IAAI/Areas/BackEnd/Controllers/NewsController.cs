using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using Ganss.Xss;
using IAAI.Models;
using IAAI.Models.ViewModels;

namespace IAAI.Areas.BackEnd.Controllers
{
    [CustomAuthorize]
    public class NewsController : Controller
    {
        private DbModel db = new DbModel();
        string title = "News"; //ViewBag.Title
        string description = "管理最新消息"; //ViewBag.Description
        private const int DefaultPageSize = 5;


        // GET: BackEnd/News
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.Title = title;
            ViewBag.Description = description;

            if (Session["newsSearch"] !=null)
            {
                var query = db.News.AsQueryable();

                query = query.Where(n => n.IsDelete == false);

                string searchJson = Session["newsSearch"].ToString();
                NewsSearchViewModel search = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsSearchViewModel>(searchJson);

                //排序
                switch (search.OrderBy)
                {
                    case "1": // 依編輯時間越新越先
                        query = query.OrderByDescending(n => n.UpdatedTime).ThenByDescending(n=>n.IsTop);
                        break;
                    case "2": // 依編輯時間越晚越先
                        query = query.OrderBy(n => n.UpdatedTime).ThenByDescending(n => n.IsTop);
                        break;
                    case "3": // 依創建時間越新越先
                        query = query.OrderByDescending(n => n.CreatedTime).ThenByDescending(n => n.IsTop);
                        break;
                    case "4": // 依創建時間越晚越先
                        query = query.OrderBy(n => n.CreatedTime).ThenByDescending(n => n.IsTop);
                        break;
                    default:
                        query = query.OrderByDescending(n => n.IsTop).ThenByDescending(n => n.UpdatedTime);
                        break;
                }

                //標題搜尋
                if (!string.IsNullOrEmpty(search.SearchTitle))
                {
                    query = query.Where(n => n.Title.Contains(search.SearchTitle));
                }

                //內文搜尋
                if (!string.IsNullOrEmpty(search.SearchContent))
                {
                    query = query.Where(n => Utility.RemoveHtmlTags(n.Content).Contains(search.SearchContent));
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



            var news = db.News
                .Where(n => n.IsDelete == false)
                .OrderByDescending(n => n.IsTop)
                .ThenByDescending(n=>n.UpdatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(news);
            //return View(db.News.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(NewsSearchViewModel search)
        {
            Session["newsSearch"]=Newtonsoft.Json.JsonConvert.SerializeObject(search);

            return RedirectToAction("Index");
        }


        // GET: BackEnd/News/Create
        public ActionResult Create(int? page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;
            return View();
        }

        // POST: BackEnd/News/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,IsTop")] News news, HttpPostedFileBase UploadImage)
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
                        return View(news);
                    }
                    news.ImageUrl = Utility.SaveUpImage(UploadImage);
                }
                else
                {
                    //未上傳圖片就給預設圖
                    news.ImageUrl = $"/UploadFiles/Images/newsimg.jpg";
                }

                var sanitizer = new HtmlSanitizer();
                news.Content = sanitizer.Sanitize(news.Content);

                //TODO:編輯者+創建者


                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: BackEnd/News/Edit/5
        public ActionResult Edit(int? id, int? page)
        {
            ViewBag.Title = title;
            ViewBag.Description = description;
            ViewBag.page = page;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: BackEnd/News/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,Title,Content,IsTop,IsDelete,ViewCount,Creator,CreatedTime")] News news, int? page, HttpPostedFileBase UploadImage)
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
                        return View(news);
                    }
                    news.ImageUrl = Utility.SaveUpImage(UploadImage);
                }

                var sanitizer = new HtmlSanitizer();
                news.Content = sanitizer.Sanitize(news.Content);

                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                page = page.HasValue ? page.Value : 1;
                return Redirect(Url.Action("Index") + $"?page={page}");
            }
            return View(news);
        }

        // POST: BackEnd/News/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int? page)
        {
            try
            {
                // 根據 id 查找要刪除的項目
                var newsItem = db.News.Find(id);
                if (newsItem == null)
                {
                    TempData["ErrorMessage"] = "刪除失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                // 刪除項目
                //db.News.Remove(newsItem);
                newsItem.IsDelete = true;
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
            //News news = db.News.Find(id);
            //db.News.Remove(news);
            //db.SaveChanges();
            //return RedirectToAction("Index");
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
