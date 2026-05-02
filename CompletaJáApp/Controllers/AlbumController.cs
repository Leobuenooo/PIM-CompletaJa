using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class AlbumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}