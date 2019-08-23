using Site.Areas.Admin.Models;
using System;
using System.Web.Mvc;

namespace Site.Areas.Admin.Controllers
{
	public class ComicsController : BaseController
	{
		[HttpGet]
		public ActionResult Index()
		{
			var lst = new Comic().GetList();
			return View(lst);
		}

		[HttpPost]
		public ContentResult Remove(int id = 0)
		{
			var rs = new Comic().Remove(id);
			return Content("{\"success\":true}", "application/json");
		}
    }
}