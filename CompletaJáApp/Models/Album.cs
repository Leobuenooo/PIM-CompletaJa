using System.ComponentModel.DataAnnotations;

namespace CompletaJaApp.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do álbum é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A quantidade total de figurinhas é obrigatória.")]
        public int QuantidadeTotalFigurinhas { get; set; }

        // Assim como no Local, guardamos apenas o caminho da capa do álbum
        [MaxLength(500)]
        public string CapaUrl { get; set; }
    }
}