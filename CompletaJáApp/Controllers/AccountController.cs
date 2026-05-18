using Microsoft.AspNetCore.Mvc;
using CompletaJaApp.Data;
using CompletaJaApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;

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
        public IActionResult Index() => View();

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
                // Vincula as informações cruciais do usuário ativo na Sessão HTTP
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
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

            if (_context.Usuarios.Any(u => u.Email == Email))
            {
                ViewBag.Erro = "Este e-mail já está em uso.";
                return View("Index");
            }

            string fotoUrl = "/images/default-avatar.png";

            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                string nomeArquivo = Guid.NewGuid().ToString() + "_" + Path.GetFileName(FotoPerfil.FileName);
                string caminhoPasta = Path.Combine(_env.WebRootPath, "images", "usuarios");

                if (!Directory.Exists(caminhoPasta)) Directory.CreateDirectory(caminhoPasta);

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