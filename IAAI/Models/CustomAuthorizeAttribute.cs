using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAI.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false; // 未登入
            }

            string userRole = IAAI.Models.Utility.GetUserRole();
            string area = httpContext.Request.RequestContext.RouteData.DataTokens["area"] as string;
            string currentController = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            // 檢查是否為後台
            if (area != null && area.Equals("BackEnd", StringComparison.OrdinalIgnoreCase) ||
                currentController.StartsWith("BackEnd", StringComparison.OrdinalIgnoreCase))
            {
                // 後台僅限 Admin
                return userRole == "Admin";
            }
            else
            {
                // 前台僅限 Member
                return userRole == "Member";
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string area = filterContext.RouteData.DataTokens["area"] as string;
            string currentController = filterContext.RouteData.Values["controller"].ToString();

            // 未授權處理
            if (area != null && area.Equals("BackEnd", StringComparison.OrdinalIgnoreCase) ||
                currentController.StartsWith("BackEnd", StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new RedirectResult("~/BackEnd/Login");
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Member/Login");
            }
        }



        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    // 取得路徑資訊
        //    string area = filterContext.RouteData.DataTokens["area"] as string; // 如果使用 Area
        //    string currentController = filterContext.RouteData.Values["controller"].ToString();

        //    // 已登入
        //    if (filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        string userRole = IAAI.Models.Utility.GetUserRole();

        //        // 檢查是否為後台
        //        if (area != null && area.Equals("BackEnd", StringComparison.OrdinalIgnoreCase) || currentController.StartsWith("BackEnd", StringComparison.OrdinalIgnoreCase))
        //        {
        //            // 判斷後台使用者角色是否為 Admin
        //            if (userRole != "Admin")
        //            {
        //                filterContext.Result = new RedirectResult("~/BackEnd/Login");
        //                return;  // 重定向後直接返回
        //            }
        //        }
        //        else
        //        {
        //            // 檢查是否為前台
        //            if (userRole != "Member")
        //            {
        //                filterContext.Result = new RedirectResult("~/Member/Login");
        //                return;  // 重定向後直接返回
        //            }
        //        }

        //        // 如果角色符合要求，讓請求繼續執行
        //        return;
        //    }

        //    // 未登入
        //    if (area != null && area.Equals("BackEnd", StringComparison.OrdinalIgnoreCase) || currentController.StartsWith("BackEnd", StringComparison.OrdinalIgnoreCase))
        //    {
        //        // 未登入且試圖訪問後台，重定向到後台登入頁
        //        filterContext.Result = new RedirectResult("~/BackEnd/Login");
        //    }
        //    else
        //    {
        //        // 未登入且試圖訪問前台，重定向到前台登入頁
        //        filterContext.Result = new RedirectResult("~/Member/Login");
        //    }
        //}
    }
}