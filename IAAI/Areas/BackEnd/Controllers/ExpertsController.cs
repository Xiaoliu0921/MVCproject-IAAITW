using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IAAI.Models;
using MvcPaging;

namespace IAAI.Areas.BackEnd.Controllers
{
    [CustomAuthorize]
    public class ExpertsController : Controller
    {
        private DbModel db = new DbModel();
        string title = "Experts"; //ViewBag.Title
        string description = "管理專家介紹"; //ViewBag.Description
        private const int DefaultPageSize = 5;

        // GET: BackEnd/Experts
        public ActionResult Index(int? page)
        {
            page = page.HasValue ? page.Value - 1 : 0;
            ViewBag.Title = title;
            ViewBag.Description = description;

            var experts = db.Experts
                .Where(n => n.IsDelete == false)
                .OrderByDescending(n => n.CreatedTime)
                .ToPagedList(page.Value, DefaultPageSize);

            return View(experts);
            //return View(db.Experts.ToList());
        }

        // GET: BackEnd/Experts/Create
        public ActionResult Create(int? page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;
            return View();
        }

        // POST: BackEnd/Experts/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImageUrl,Name,CurrentPosition")] Expert expert, HttpPostedFileBase UploadImage, List<string> Education, List<string> Introduction, List<string> Experience, List<string> Expertise, List<string> AppraisalExperience, List<string> AcademicPublications, List<string> ResearchProjects, List<string> TrainingOrCertifications, List<string> DetailedExperience, List<string> ConcurrentPublicSector, List<string> ConcurrentLegalAgency)
        {
            if (ModelState.IsValid)
            {
                //判斷有沒有上傳預覽圖
                if (UploadImage != null)
                {
                    //判斷是不是圖檔
                    if (UploadImage.ContentType.IndexOf("image", System.StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        return View(expert);
                    }
                    expert.ImageUrl = Utility.SaveUpImage(UploadImage);
                }
                else
                {
                    //未上傳圖片就給預設圖
                    expert.ImageUrl = $"/UploadFiles/Images/profile.jpg";
                }

                #region 各項目的處理
                if (Education != null && Education.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Education = string.Join(",,,", Education.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Education = string.Empty; // 如果沒有輸入值
                }

                if (Introduction != null && Introduction.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Introduction = string.Join(",,,", Introduction.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Introduction = string.Empty; // 如果沒有輸入值
                }

                if (Experience != null && Experience.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Experience = string.Join(",,,", Experience.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Experience = string.Empty; // 如果沒有輸入值
                }

                if (Expertise != null && Expertise.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Expertise = string.Join(",,,", Expertise.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Expertise = string.Empty; // 如果沒有輸入值
                }

                if (AppraisalExperience != null && AppraisalExperience.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.AppraisalExperience = string.Join(",,,", AppraisalExperience.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.AppraisalExperience = string.Empty; // 如果沒有輸入值
                }

                if (AcademicPublications != null && AcademicPublications.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.AcademicPublications = string.Join(",,,", AcademicPublications.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.AcademicPublications = string.Empty; // 如果沒有輸入值
                }

                if (ResearchProjects != null && ResearchProjects.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.ResearchProjects = string.Join(",,,", ResearchProjects.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.ResearchProjects = string.Empty; // 如果沒有輸入值
                }

                if (TrainingOrCertifications != null && TrainingOrCertifications.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.TrainingOrCertifications = string.Join(",,,", TrainingOrCertifications.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.TrainingOrCertifications = string.Empty; // 如果沒有輸入值
                }

                if (DetailedExperience != null && DetailedExperience.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.DetailedExperience = string.Join(",,,", DetailedExperience.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.DetailedExperience = string.Empty; // 如果沒有輸入值
                }

                if (ConcurrentPublicSector != null && ConcurrentPublicSector.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.ConcurrentPublicSector = string.Join(",,,", ConcurrentPublicSector.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.ConcurrentPublicSector = string.Empty; // 如果沒有輸入值
                }

                if (ConcurrentLegalAgency != null && ConcurrentLegalAgency.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.ConcurrentLegalAgency = string.Join(",,,", ConcurrentLegalAgency.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.ConcurrentLegalAgency = string.Empty; // 如果沒有輸入值
                }
                #endregion


                db.Experts.Add(expert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expert);
        }

        // GET: BackEnd/Experts/Edit/5
        public ActionResult Edit(int? id, int? page)
        {
            ViewBag.page = page;
            ViewBag.Title = title;
            ViewBag.Description = description;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        // POST: BackEnd/Experts/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImageUrl,Name,CurrentPosition,IsDelete,Creator,CreatedTime")] Expert expert,int? page, HttpPostedFileBase UploadImage, List<string> Education, List<string> Introduction, List<string> Experience, List<string> Expertise, List<string> AppraisalExperience, List<string> AcademicPublications, List<string> ResearchProjects, List<string> TrainingOrCertifications, List<string> DetailedExperience, List<string> ConcurrentPublicSector, List<string> ConcurrentLegalAgency)
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
                        return View(expert);
                    }
                    expert.ImageUrl = Utility.SaveUpImage(UploadImage);
                }

                #region 各項目的處理
                if (Education != null && Education.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Education = string.Join(",,,", Education.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Education = string.Empty; // 如果沒有輸入值
                }

                if (Introduction != null && Introduction.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Introduction = string.Join(",,,", Introduction.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Introduction = string.Empty; // 如果沒有輸入值
                }

                if (Experience != null && Experience.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Experience = string.Join(",,,", Experience.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Experience = string.Empty; // 如果沒有輸入值
                }

                if (Expertise != null && Expertise.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.Expertise = string.Join(",,,", Expertise.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.Expertise = string.Empty; // 如果沒有輸入值
                }

                if (AppraisalExperience != null && AppraisalExperience.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.AppraisalExperience = string.Join(",,,", AppraisalExperience.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.AppraisalExperience = string.Empty; // 如果沒有輸入值
                }

                if (AcademicPublications != null && AcademicPublications.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.AcademicPublications = string.Join(",,,", AcademicPublications.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.AcademicPublications = string.Empty; // 如果沒有輸入值
                }

                if (ResearchProjects != null && ResearchProjects.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.ResearchProjects = string.Join(",,,", ResearchProjects.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.ResearchProjects = string.Empty; // 如果沒有輸入值
                }

                if (TrainingOrCertifications != null && TrainingOrCertifications.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.TrainingOrCertifications = string.Join(",,,", TrainingOrCertifications.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.TrainingOrCertifications = string.Empty; // 如果沒有輸入值
                }

                if (DetailedExperience != null && DetailedExperience.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.DetailedExperience = string.Join(",,,", DetailedExperience.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.DetailedExperience = string.Empty; // 如果沒有輸入值
                }

                if (ConcurrentPublicSector != null && ConcurrentPublicSector.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.ConcurrentPublicSector = string.Join(",,,", ConcurrentPublicSector.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.ConcurrentPublicSector = string.Empty; // 如果沒有輸入值
                }

                if (ConcurrentLegalAgency != null && ConcurrentLegalAgency.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // 過濾掉空值或只包含空白的字串，然後用逗號分隔
                    expert.ConcurrentLegalAgency = string.Join(",,,", ConcurrentLegalAgency.Where(x => !string.IsNullOrWhiteSpace(x)));
                }
                else
                {
                    expert.ConcurrentLegalAgency = string.Empty; // 如果沒有輸入值
                }
                #endregion

                db.Entry(expert).State = EntityState.Modified;
                db.SaveChanges();
                page = page.HasValue ? page.Value : 1;
                return Redirect(Url.Action("Index") + $"?page={page}");
            }
            return View(expert);
        }

        //// GET: BackEnd/Experts/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    ViewBag.Title = title;
        //    ViewBag.Description = description;
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Expert expert = db.Experts.Find(id);
        //    if (expert == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(expert);
        //}

        // POST: BackEnd/Experts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int? page)
        {
            try
            {
                // 根據 id 查找要刪除的項目
                var expertItem = db.Experts.Find(id);
                if (expertItem == null)
                {
                    TempData["ErrorMessage"] = "刪除失敗！";
                    return RedirectToAction("Index", new { page = page });
                }

                // 刪除項目
                expertItem.IsDelete = true;
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
