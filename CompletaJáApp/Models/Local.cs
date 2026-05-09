using System.ComponentModel.DataAnnotations;

namespace CompletaJaApp.Models
{
    public class Local
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do local é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [MaxLength(20)]
        public string CNPJ { get; set; }

        [MaxLength(255)]
        public string PontoDeEncontro { get; set; }

        // Adicionamos o campo para guardar o endereço da imagem!
        [MaxLength(500)]
        public string FotoUrl { get; set; }
    }
}