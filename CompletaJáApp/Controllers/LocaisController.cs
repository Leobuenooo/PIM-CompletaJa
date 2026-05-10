using Microsoft.AspNetCore.Mvc;

namespace CompletaJaApp.Controllers
{
    public class LocaisController : Controller
    {
        // Abre a página Meus Locais
        public IActionResult Index()
        {
            return View();
        }

        // Abre a página de Adicionar Local
        public IActionResult Add()
        {
            return View();
        }

        // Salva o local e volta pra tela inicial
        [HttpPost]
        public IActionResult Criar()
        {
            return RedirectToAction("Index");
        }
    }
}