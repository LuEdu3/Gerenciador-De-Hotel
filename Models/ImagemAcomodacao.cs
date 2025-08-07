using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public class ImagemAcomodacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Acomodação")]
        public int AcomodacaoId { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória")]
        [StringLength(255, ErrorMessage = "A URL deve ter no máximo 255 caracteres")]
        [Display(Name = "URL da Imagem")]
        public string ImagemUrl { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        [Display(Name = "Título da Imagem")]
        public string? Titulo { get; set; }

        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Ordem de Exibição")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; } = true;

        [Display(Name = "Data de Upload")]
        public DateTime DataUpload { get; set; } = DateTime.Now;

        // Relacionamentos
        [ForeignKey("AcomodacaoId")]
        public virtual Acomodacao Acomodacao { get; set; } = null!;
    }
}
