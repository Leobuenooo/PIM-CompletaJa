using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // COMENTADO TEMPORARIAMENTE PARA TESTES
            // string usuario = HttpContext.Session.GetString("UsuarioLogado");
            // if (string.IsNullOrEmpty(usuario))
            // {
            //     return RedirectToAction("Index", "Account");
            // }

            return View();
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
    }
}