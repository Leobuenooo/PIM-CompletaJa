using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string usuario = HttpContext.Session.GetString("NomeUsuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
    }
}