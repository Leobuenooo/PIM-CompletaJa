using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompletaJaApp.Models
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do álbum é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; }

        // Esta anotação força o C# a salvar os dados na coluna antiga do banco
        [Column("TotalFigurinhas")]
        [Required(ErrorMessage = "A quantidade total de figurinhas é obrigatória.")]
        public int QuantidadeTotalFigurinhas { get; set; }

        [MaxLength(500)]
        public string CapaUrl { get; set; }
    }
}