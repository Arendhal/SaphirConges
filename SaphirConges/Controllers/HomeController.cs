using System.Web.Mvc;

namespace SaphirConges.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Saphir -- Congés";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A propos de l'application";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Page de contact";
            return View();
        }
    }
}