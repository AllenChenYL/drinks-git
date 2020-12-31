using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Drinks.Filters
{
    public class LoginAuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //未登入導向登入頁面
            if (!HttpContext.Current.User.Identity.IsAuthenticated) 
            {
                filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary 
                                   {
                                       { "action", "Login" },
                                       { "controller", "Account" }
                                   });
            }
            base.OnActionExecuted(filterContext);
        }
    }
}