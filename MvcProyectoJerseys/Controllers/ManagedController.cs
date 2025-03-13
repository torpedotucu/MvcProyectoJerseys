using Microsoft.AspNetCore.Mvc;

namespace MvcProyectoJerseys.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
