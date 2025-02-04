using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;


namespace IAAI.Models
{
    public class PermissionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            DbModel db = new DbModel();

            //判斷是否有登入(表單驗證有通過)
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.SideBar = "";

                return;
            }

            //取得UserData
            string strUserData = ((FormsIdentity)(HttpContext.Current.User.Identity)).Ticket.UserData;
            Member member = JsonConvert.DeserializeObject<Member>(strUserData);
            //取得資料庫的現在資料
            Member memberData = db.Members.Find(member.Id);

            ////取得使用者的權限
            //string userPermissions = memberData.NewPermission;
            ////取得符合使用者的權限資料
            //var permissions = db.Permissions.Where(x => userPermissions.Contains(x.Code)).ToList();
            ////判斷使用者是否有當下的Controller權限
            //string controllerName = filterContext.RouteData.Values["controller"].ToString();
            ////若無則登出
            //if (!permissions.Any(x => x.ControllerName == controllerName))
            //{
            //    FormsAuthentication.SignOut();
            //    filterContext.Result = new RedirectResult("~/Login/Index");
            //    return;
            //}

            //TODO:產生側邊欄
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"nav\">");

        }
    }
}