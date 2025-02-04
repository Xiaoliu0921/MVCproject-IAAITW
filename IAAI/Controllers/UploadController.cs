using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        [Route("/UploadFiles/Images")]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum)
        {
            try
            {
                // 確認上傳的檔案是否存在
                if (upload != null && upload.ContentLength > 0)
                {
                    if (upload.ContentLength > 5 * 1024 * 1024)
                    {
                        throw new Exception("檔案大小不可超過 5MB。");
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (!allowedExtensions.Contains(Path.GetExtension(upload.FileName).ToLower()))
                    {
                        throw new Exception("只允許上傳圖片格式檔案。");
                    }


                    // 設定圖片存放路徑
                    var uploadDir = Server.MapPath("~/UploadFiles/Images");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir); // 確保目錄存在
                    }

                    // 生成唯一的檔名避免覆蓋
                    var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    // 儲存檔案
                    upload.SaveAs(filePath);

                    // 返回圖片的完整路徑（URL 路徑）
                    var fileUrl = Url.Content("~/UploadFiles/Images/" + fileName);

                    // CKEditor 的回應格式
                    return Content($"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{fileUrl}', '');</script>");
                }
                else
                {
                    throw new Exception("未選擇文件或文件無內容。");
                }
            }
            catch (Exception ex)
            {
                // 返回錯誤資訊
                return Content($"<script>alert('上傳失敗: {ex.Message}');</script>");
            }
        }
    }
}