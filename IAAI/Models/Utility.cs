using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web.Helpers;
using System.Xml.Linq;
using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace IAAI.Models
{
    public class Utility
    {

        #region "密碼加密"
        public const int DefaultSaltSize = 5;
        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Computes a salted hash of the password and salt provided and returns as a base64 encoded string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }

        #endregion

        #region "將使用者資料寫入cookie,產生AuthenTicket"
        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        static public void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將Cookie寫入回應

            HttpContext.Current.Response.Cookies.Add(authenticationcookie);

        }
        #endregion

        #region 取得UserData

        public static string GetUserData()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                return ticket?.UserData; // 如果 ticket 不為 null，則返回 UserData
            }
            return null;
        }

        public static string GetUserRole()
        {
            string userDataJson = GetUserData();
            if (userDataJson != null)
            {
                // 反序列化 UserData
                var userData = JsonConvert.DeserializeObject<dynamic>(userDataJson);

                // 使用資料
                string role = userData.Role;
                return role;
            }

            return null;
           
        }

        public static int GetUserId()
        {
            string userDataJson = GetUserData();
            // 反序列化 UserData
            var userData = JsonConvert.DeserializeObject<dynamic>(userDataJson);

            // 使用資料
            int id = userData.Id;
            return id;
        }

        #endregion

        #region"儲存上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="upfile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        static public string SaveUpImage(HttpPostedFileBase upfile)
        {
            //儲存位址
            string folderPath = "/UploadFiles/Images/";

            //取得副檔名
            string extension = upfile.FileName.Split('.')[upfile.FileName.Split('.').Length - 1];
            //新檔案名稱
            string fileName = String.Format("{0:yyyyMMddhhmmsss}.{1}", DateTime.Now, extension);
            string savedName = Path.Combine(HttpContext.Current.Server.MapPath($"~{folderPath}"), fileName);
            upfile.SaveAs(savedName);
            //return fileName;
            return folderPath + fileName;
        }
        #endregion

        #region"儲存上傳檔案"
        /// <summary>
        /// 儲存上傳檔案
        /// </summary>
        /// <param name="upfile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        static public string SaveUpFile(HttpPostedFileBase upfile)
        {
            //儲存位址
            string folderPath = "/UploadFiles/";

            //取得副檔名
            string extension = upfile.FileName.Split('.')[upfile.FileName.Split('.').Length - 1];
            //新檔案名稱
            string fileName = String.Format("{0:yyyyMMddhhmmsss}.{1}", DateTime.Now, extension);
            string savedName = Path.Combine(HttpContext.Current.Server.MapPath($"~{folderPath}"), fileName);
            upfile.SaveAs(savedName);
            return folderPath + fileName;
        }
        #endregion

        #region "驗證E-mail格式"
        /// <summary>
        /// 驗證E-mail格式
        /// </summary>
        /// <param name="strIn">輸入E-mail</param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region 刪除Html標籤
        public static string RemoveHtmlTags(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // 將 HTML 實體（例如 &nbsp;）解碼為普通字元
            string decoded = WebUtility.HtmlDecode(input);

            // 移除 HTML 標籤
            string noHtml = Regex.Replace(decoded, "<.*?>", string.Empty);

            // 刪除多餘的空白
            return Regex.Replace(noHtml, @"\s+", " ").Trim();
        }
        #endregion

        #region "寄送Gmail"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="senderName">發信者名稱</param>
        /// <param name="fromAddress">發信者mail</param>
        /// <param name="recipientName">收信者名稱</param>
        /// <param name="toAddress">收信者mail</param>
        /// <param name="subject">主旨</param>
        /// <param name="mailBody">信件內容</param>
        public static void SendGmail(string senderName, string fromAddress, string recipientName, string toAddress, string subject, string mailBody)
        {
            //宣告使用 MimeMessage
            var message = new MimeMessage();
            //設定發信地址 ("發信人", "發信 email")
            message.From.Add(new MailboxAddress(senderName, fromAddress));
            //設定收信地址 ("收信人", "收信 email")
            message.To.Add(new MailboxAddress(recipientName.Trim(), toAddress.Trim()));
            //寄件副本email
            message.Cc.Add(new MailboxAddress("管理員", "**********@gmail.com"));
            //設定優先權
            //message.Priority = MessagePriority.Normal;
            //信件標題
            message.Subject = subject;
            //建立 html 郵件格式
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailBody;
            //設定郵件內容
            message.Body = bodyBuilder.ToMessageBody(); //轉成郵件內容格式

            using (var client = new SmtpClient())
            {
                //有開防毒時需設定 false 關閉檢查
                client.CheckCertificateRevocation = false;
                //設定連線 gmail ("smtp Server", Port, SSL加密) 
                client.Connect("smtp.gmail.com", 587, false); // localhost 測試使用加密需先關閉 

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("***********@gmail.com", "****************");
                //發信
                client.Send(message);
                //結束連線
                client.Disconnect(true);
            }
        }
        #endregion

    }
}