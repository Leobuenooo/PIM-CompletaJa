using Microsoft.AspNetCore.Http; // ADICIONADO: Necessário para ler a Session
using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // 1. Lemos o "crachá" do usuário
            string usuario = HttpContext.Session.GetString("NomeUsuario");

            // 2. SEGURANÇA: Se o crachá estiver vazio (não logado), chuta de volta pro Login
            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Account");
            }

            // 3. Se passou pela trava, exibe a tela Home normalmente
            return View();
        }

        public IActionResult Sair()
        {
            // Apaga o "crachá" da memória do servidor
            HttpContext.Session.Clear();

            // Redireciona para o Login
            return RedirectToAction("Index", "Account");
        }
    }
}