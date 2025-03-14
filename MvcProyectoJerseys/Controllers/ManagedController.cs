using Microsoft.AspNetCore.Mvc;
using MvcProyectoJerseys.Repositories;

namespace MvcProyectoJerseys.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryCamisetas repo;
        public ManagedController(RepositoryCamisetas repo)
        {
            this.repo=repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(string username,string password)
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
