namespace Warehouse_23DH113071.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Products");
        }
    }
}
