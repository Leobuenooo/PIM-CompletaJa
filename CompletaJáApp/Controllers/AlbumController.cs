using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class AlbumController : Controller
    {
        // Esta é a página principal de Álbuns
        public IActionResult Index()
        {
            return View();
        }

        // ADICIONE ESTE MÉTODO PARA A NOVA PÁGINA
        public IActionResult Add()
        {
            return View();
        }
    }
}