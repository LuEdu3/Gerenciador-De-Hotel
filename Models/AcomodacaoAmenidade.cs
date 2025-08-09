using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public class AcomodacaoAmenidade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Acomodação")]
        public int AcomodacaoId { get; set; }

        [Required]
        [Display(Name = "Amenidade")]
        public int AmenidadeId { get; set; }

        [Display(Name = "Data de Associação")]
        public DateTime DataAssociacao { get; set; } = DateTime.Now;

        // Relacionamentos
        [ForeignKey("AcomodacaoId")]
        public virtual Acomodacao Acomodacao { get; set; } = null!;

        [ForeignKey("AmenidadeId")]
        public virtual Amenidade Amenidade { get; set; } = null!;
    }
}
