using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public enum StatusAcomodacao
    {
        [Display(Name = "Disponível")]
        Disponivel = 1,
        [Display(Name = "Ocupada")]
        Ocupada = 2,
        [Display(Name = "Manutenção")]
        Manutencao = 3,
        [Display(Name = "Fora de Serviço")]
        ForaDeServico = 4
    }

    public class Acomodacao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da acomodação é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome da Acomodação")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }
    [Required(ErrorMessage = "A quantidade de camas é obrigatória")]
    [Range(1, 10, ErrorMessage = "A quantidade de camas deve ser entre 1 e 10")]
    [Display(Name = "Quantidade de Camas")]
    public int QuantidadeCamas { get; set; }

    [Range(0, 10, ErrorMessage = "A quantidade de camas de casal deve ser entre 0 e 10")]
    [Display(Name = "Quantidade de Camas de Casal")]
    public int QuantidadeCamasCasal { get; set; } = 0;

    [NotMapped]
    [Display(Name = "Quantidade de Camas de Solteiro")]
    public int QuantidadeCamasSolteiro => QuantidadeCamas - QuantidadeCamasCasal;


        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 9999.99, ErrorMessage = "O preço deve ser maior que zero")]
        [Display(Name = "Preço por Noite")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O mínimo de noites é obrigatório")]
        [Range(1, 30, ErrorMessage = "O mínimo de noites deve ser entre 1 e 30")]
        [Display(Name = "Mínimo de Noites")]
        public int MinimoNoites { get; set; } = 1;

        [Display(Name = "Status")]
        public StatusAcomodacao Status { get; set; } = StatusAcomodacao.Disponivel;

        [Display(Name = "URL da Imagem Principal")]
        [StringLength(255)]
        public string? ImagemPrincipalUrl { get; set; }

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        // Relacionamentos
        public virtual ICollection<AcomodacaoAmenidade> AcomodacaoAmenidades { get; set; } = new List<AcomodacaoAmenidade>();
        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
        public virtual ICollection<ImagemAcomodacao> Imagens { get; set; } = new List<ImagemAcomodacao>();
    }
}
