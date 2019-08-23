using Site.Models;
using System.Web.Mvc;
using System;
using Site.Areas.Admin.Models;

namespace Site.Controllers
{
	public class ComicController : Controller
	{
        [HttpGet]
        public ActionResult Index()
        {
            var idCharacter = Request.QueryString.Count == 0 ? 0 : Convert.ToInt32(Request.QueryString[0]);

            ViewBag.Character = new CharacterMarvel().GetCharacterMarvel(idCharacter);
            var lst = new ComicMarvel().GetComicMarvel(idCharacter);
            return View(lst);
        }

        public ContentResult Like(int id = 0, string name = "")
        {
            var rs = new Comic().Like(id, name.Replace("*", "#"));
            return Content("{\"success\":true}", "application/json");
        }
    }
}