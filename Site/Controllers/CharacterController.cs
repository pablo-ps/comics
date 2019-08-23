using Site.Models;
using System.Web.Mvc;

namespace Site.Controllers
{
	public class CharacterController : Controller
	{
        [HttpGet]
        public ActionResult Index()
		{
            var textSearch = Request.QueryString.Count == 0 ? "" : Request.QueryString[0].ToString();
            var lst = new CharacterMarvel().GetCharactersMarvel(textSearch);
            return View(lst);
		}
    }
}