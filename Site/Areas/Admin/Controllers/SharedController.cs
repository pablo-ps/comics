using Site.Areas.Admin.Models;
using System;
using System.Web.Mvc;

namespace Site.Areas.Admin.Controllers
{
	public class SharedController : BaseController
	{
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Logout()
		{
			Session.Abandon();
			Session.Clear();
			CurrentUser = null;
			return Redirect("/Admin/Shared/Login");
		}

		[HttpPost]
		public ActionResult CheckLogin(string email, string password)
		{
			CurrentUser = new User().CheckLogin(email, password);
			return Redirect(CurrentUser?.Id > 0 ? "/Admin/Comics" : "/Admin/Shared/Login");
		}

		[HttpPost]
		public ContentResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
		{
			var rs = false;
			var msg = "";
			try
			{
				rs = new User().ChangePassword(CurrentUser.Email, currentPassword, newPassword, confirmNewPassword);
			}
			catch (Exception ex)
			{
				msg = ex.Message;
			}
			return Content("{\"success\":" + (rs ? "true" : "false") + ", \"msg\":\"" + msg + "\"}", "application/json");
		}
	}
}