using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public enum StatusPagamento
    {
        [Display(Name = "Pendente")]
        Pendente = 1,
        [Display(Name = "Pago")]
        Pago = 2,
        [Display(Name = "Cancelado")]
        Cancelado = 3,
        [Display(Name = "Estornado")]
        Estornado = 4
    }

    public enum TipoPagamento
    {
        [Display(Name = "Dinheiro")]
        Dinheiro = 1,
        [Display(Name = "Cartão de Crédito")]
        CartaoCredito = 2,
        [Display(Name = "Cartão de Débito")]
        CartaoDebito = 3,
        [Display(Name = "PIX")]
        Pix = 4,
        [Display(Name = "Transferência Bancária")]
        TransferenciaBancaria = 5
    }

    public class Pagamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Reserva")]
        public int ReservaId { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, 999999.99, ErrorMessage = "O valor deve ser maior que zero")]
        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Display(Name = "Tipo de Pagamento")]
        public TipoPagamento TipoPagamento { get; set; }

        [Display(Name = "Status do Pagamento")]
        public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;

        [Display(Name = "Data do Pagamento")]
        public DateTime DataPagamento { get; set; } = DateTime.Now;

        [StringLength(100, ErrorMessage = "A descrição deve ter no máximo 100 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(255, ErrorMessage = "A referência externa deve ter no máximo 255 caracteres")]
        [Display(Name = "Referência Externa")]
        public string? ReferenciaExterna { get; set; }

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres")]
        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        // Relacionamentos
        [ForeignKey("ReservaId")]
        public virtual Reserva Reserva { get; set; } = null!;
    }
}
