using Microsoft.AspNetCore.Mvc;
using CompletaJaApp.Data;
using CompletaJaApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CompletaJáApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly CompletaJaContext _context;
        private readonly IWebHostEnvironment _env;

        public AlbumController(CompletaJaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            // Busca a foto da Sessão. Se não achar, coloca um avatar padrão.
            ViewBag.FotoUsuario = HttpContext.Session.GetString("FotoUsuario") ?? "/images/default-avatar.png";

            var albunsDoBanco = _context.Albuns.ToList();
            return View(albunsDoBanco);
        }

        public IActionResult Add()
        {
            ViewBag.FotoUsuario = HttpContext.Session.GetString("FotoUsuario") ?? "/images/default-avatar.png";

            // Busca os 5 álbuns mais recentes (Futuramente, mudaremos para ranking de mais colecionados)
            var sugestoes = _context.Albuns.OrderByDescending(a => a.Id).Take(5).ToList();

            return View(sugestoes);
        }

        [HttpPost]
        public IActionResult VerificarDuplicidade(string nome, int quantidade)
        {
            if (string.IsNullOrWhiteSpace(nome) || quantidade <= 0)
                return Json(new { duplicado = false });

            var albunsMesmaQuantidade = _context.Albuns.Where(a => a.QuantidadeTotalFigurinhas == quantidade).ToList();

            if (!albunsMesmaQuantidade.Any())
                return Json(new { duplicado = false });

            var palavrasNovoNome = nome.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Where(p => p.Length > 2).ToList();

            foreach (var album in albunsMesmaQuantidade)
            {
                var palavrasAlbumBanco = album.Nome.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                             .Where(p => p.Length > 2).ToList();

                if (palavrasNovoNome.Intersect(palavrasAlbumBanco).Any())
                {
                    return Json(new { duplicado = true, nomeSemelhante = album.Nome });
                }
            }

            return Json(new { duplicado = false });
        }

        [HttpPost]
        public async Task<IActionResult> Criar(string Nome, string Categoria, int TotalFigurinhas, IFormFile Capa)
        {
            if (string.IsNullOrWhiteSpace(Nome) || string.IsNullOrWhiteSpace(Categoria) || TotalFigurinhas <= 0 || Capa == null || Capa.Length == 0)
            {
                return RedirectToAction("Add");
            }

            string nomeArquivo = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Capa.FileName);
            string caminhoPasta = Path.Combine(_env.WebRootPath, "images", "albuns");

            if (!Directory.Exists(caminhoPasta))
            {
                Directory.CreateDirectory(caminhoPasta);
            }

            string caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await Capa.CopyToAsync(stream);
            }

            var novoAlbum = new Album
            {
                Nome = Nome,
                QuantidadeTotalFigurinhas = TotalFigurinhas,
                CapaUrl = "/images/albuns/" + nomeArquivo
            };

            _context.Albuns.Add(novoAlbum);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}