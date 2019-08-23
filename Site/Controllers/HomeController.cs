using Site.Areas.Admin.Models;
using System.Web.Mvc;

namespace Site.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
            ViewBag.ComicPorcents = new Comic().GetListPorcentLike();
            return View();
		}
	}
}