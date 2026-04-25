using Microsoft.AspNetCore.Mvc;

namespace CompletaJáApp.Controllers
{
    public class AccountController : Controller
    {
        // 1. TELA INICIAL (Login / Cadastro)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // 2. RECEBE OS DADOS DE LOGIN
        [HttpPost]
        public IActionResult Login(string Email, string Senha)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha))
            {
                ViewBag.Erro = "Preencha todos os campos.";
                return View("Index");
            }

            // Simulação para o PIM
            if (Email == "aluno@unip.br" && Senha == "pim2026")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View("Index");
        }

        // 3. RECEBE OS DADOS DE CRIAR CONTA
        [HttpPost]
        public IActionResult Register(string Nome, string Email, string Senha, string ConfirmaSenha)
        {
            if (Senha != ConfirmaSenha)
            {
                ViewBag.Erro = "As senhas não coincidem.";
                return View("Index");
            }

            return RedirectToAction("Index", "Account");
        }

        // 4. TELA DE TERMOS DE USO
        [HttpGet]
        public IActionResult Termos()
        {
            return View();
        }

        // 5. TELA DE ESQUECEU A SENHA (GET - Abrir a tela)
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // 6. TELA DE ESQUECEU A SENHA (POST - Enviar o formulário)
        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                ViewBag.Erro = "Por favor, informe seu e-mail.";
                return View();
            }

            ViewBag.Sucesso = $"Se o e-mail {Email} estiver cadastrado, enviaremos um link de recuperação.";
            return View();
        }
    }
}