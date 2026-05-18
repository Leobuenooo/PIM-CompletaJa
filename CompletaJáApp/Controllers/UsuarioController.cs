using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CompletaJaApp.Data;
using CompletaJaApp.Models;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace CompletaJáApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly CompletaJaContext _context;
        private readonly IWebHostEnvironment _environment;

        public UsuarioController(CompletaJaContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Exibe a tela de perfil preenchida
        [HttpGet]
        public IActionResult Editar()
        {
            int? meuId = HttpContext.Session.GetInt32("UsuarioId");
            if (meuId == null) return RedirectToAction("Login", "Home");

            var usuario = _context.Usuarios.Find(meuId.Value);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        // POST: Recebe apenas a nova senha e a nova foto para salvar com segurança
        // CORREÇÃO: Parâmetros tipados como anuláveis (?) para bater com as configurações do C# moderno
        [HttpPost]
        public IActionResult Salvar(string? NovaSenha, IFormFile? NovaFoto)
        {
            int? meuId = HttpContext.Session.GetInt32("UsuarioId");
            if (meuId == null) return RedirectToAction("Login", "Home");

            var usuario = _context.Usuarios.Find(meuId.Value);
            if (usuario == null) return NotFound();

            // 1. CORREÇÃO: Atualiza a propriedade 'SenhaHash' (nome idêntico ao mapeado na Model)
            if (!string.IsNullOrWhiteSpace(NovaSenha))
            {
                usuario.SenhaHash = NovaSenha;
            }

            // 2. Processa o upload físico da imagem se enviada
            if (NovaFoto != null && NovaFoto.Length > 0)
            {
                // Garante que a pasta física de perfis exista no servidor
                string pastaImagens = Path.Combine(_environment.WebRootPath, "images", "perfis");
                if (!Directory.Exists(pastaImagens))
                {
                    Directory.CreateDirectory(pastaImagens);
                }

                // Padroniza o nome do arquivo usando o ID do usuário para nunca duplicar ou quebrar links
                string nomeArquivo = $"perfil_{usuario.Id}{Path.GetExtension(NovaFoto.FileName)}";
                string caminhoCompleto = Path.Combine(pastaImagens, nomeArquivo);

                // Grava o arquivo físico em disco
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    NovaFoto.CopyTo(stream);
                }

                // Grava o caminho virtual relativo no registro do banco
                usuario.FotoUrl = $"/images/perfis/{nomeArquivo}";

                // Atualiza instantaneamente a variável de Sessão para atualizar o Header global do _Layout
                HttpContext.Session.SetString("FotoUsuario", usuario.FotoUrl);
            }

            _context.SaveChanges();

            // Define uma mensagem de feedback que dura apenas um ciclo de renderização
            TempData["MensagemSucesso"] = "Perfil updated com sucesso!";

            return RedirectToAction("Editar");
        }
    }
}