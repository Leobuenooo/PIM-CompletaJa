using Microsoft.AspNetCore.Mvc;
using CompletaJaApp.Data;
using CompletaJaApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http; // ADICIONADO: Necessário para usar Session

namespace CompletaJáApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly CompletaJaContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(CompletaJaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // 3. RECEBE OS DADOS DE LOGIN (Atualizado com Sessão)
        [HttpPost]
        public IActionResult Login(string Email, string Senha)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha))
            {
                ViewBag.Erro = "Preencha todos os campos.";
                return View("Index");
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == Email && u.SenhaHash == Senha);

            if (usuario != null)
            {
                // --- AQUI ESTÁ A ALTERAÇÃO ---
                // Guardamos o Nome e o caminho da Foto na Sessão para usar na Home
                HttpContext.Session.SetString("NomeUsuario", usuario.Nome);
                HttpContext.Session.SetString("FotoUsuario", usuario.FotoUrl);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos!";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string Nome, string Email, string CPF, string Senha, string ConfirmaSenha, IFormFile FotoPerfil)
        {
            if (Senha != ConfirmaSenha)
            {
                ViewBag.Erro = "As senhas não coincidem.";
                return View("Index");
            }

            var emailJaExiste = _context.Usuarios.Any(u => u.Email == Email);
            if (emailJaExiste)
            {
                ViewBag.Erro = "Este e-mail já está em uso.";
                return View("Index");
            }

            string fotoUrl = "/images/default-avatar.png";

            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                string nomeArquivo = Guid.NewGuid().ToString() + "_" + Path.GetFileName(FotoPerfil.FileName);
                string caminhoPasta = Path.Combine(_env.WebRootPath, "images", "usuarios");

                if (!Directory.Exists(caminhoPasta))
                {
                    Directory.CreateDirectory(caminhoPasta);
                }

                string caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await FotoPerfil.CopyToAsync(stream);
                }

                fotoUrl = "/images/usuarios/" + nomeArquivo;
            }

            var novoUsuario = new Usuario
            {
                Nome = Nome,
                Email = Email,
                CPF = CPF,
                SenhaHash = Senha,
                FotoUrl = fotoUrl
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            TempData["Sucesso"] = "Conta criada com sucesso! Faça seu login.";
            return RedirectToAction("Index", "Account");
        }

        // Métodos de apoio (Termos, Esqueci Senha) permanecem iguais...
        [HttpGet]
        public IActionResult Termos() => View();

        [HttpGet]
        public IActionResult ForgotPassword() => View();

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