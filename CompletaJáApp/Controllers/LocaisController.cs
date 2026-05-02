using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class LocaisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}