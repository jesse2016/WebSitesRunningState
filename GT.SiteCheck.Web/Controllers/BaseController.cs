using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GT.SiteCheck.Web.Controllers
{
    public class BaseController : Controller
    {
        public string userName = string.Empty;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var reurl = filterContext.HttpContext.Request.Url == null ? "#" : filterContext.HttpContext.Request.Url.PathAndQuery;
            userName = Session["userName"] == null ? "" : Session["userName"].ToString();
            if (userName == string.Empty)
            {
                filterContext.Result = RedirectToAction("Index", "User", new { ReturnUrl = reurl });
            }
            else
            {
                userName = Session["userName"].ToString();
            }
        }
    }
}