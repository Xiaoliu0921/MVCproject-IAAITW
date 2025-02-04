using System.Web.Mvc;

namespace IAAI.Areas.BackEnd
{
    public class BackEndAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BackEnd";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BackEnd_default",
                "BackEnd/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IAAI.Areas.BackEnd.Controllers" } // 指定後台命名空間
            );
        }
    }
}