using Site.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Site.Areas.Admin.Controllers
{
	public class BaseController : Controller
	{
		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			var url = Request.RawUrl;

			var lstPages = new List<string> { "/Admin/Shared/Login", "/Admin/Shared/Logout", "/Admin/Shared/CheckLogin" };

			var loginRequired = url != "/" && !lstPages.Any(page => url.StartsWith(page));
			if (loginRequired && CurrentUser == null)
			{
				if (filterContext.HttpContext.Request.IsAjaxRequest())
				{
					filterContext.HttpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Forbidden);
					filterContext.Result = new JsonResult() { Data = new { Message = (CurrentUser == null ? "Session has expired" : "Access denied") }, JsonRequestBehavior = JsonRequestBehavior.AllowGet, ContentType = "application/json" };
				}
				else
				{
					filterContext.HttpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.Redirect);
					filterContext.Result = new RedirectResult("~/Admin/Shared/Login");
				}
				return;
			}

			base.OnAuthorization(filterContext);
		}

		public User CurrentUser
		{
			get { return Session["User"] as User; }
			set { Session["User"] = value; }
		}
	}
}