using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public class Amenidade
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da amenidade é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome da Amenidade")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "URL da Imagem")]
        [StringLength(255)]
        public string? ImagemUrl { get; set; }

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Relacionamentos
        public virtual ICollection<AcomodacaoAmenidade> AcomodacaoAmenidades { get; set; } = new List<AcomodacaoAmenidade>();
    }
}
