using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public enum StatusReserva
    {
        [Display(Name = "Pendente")]
        Pendente = 1,
        [Display(Name = "Confirmada")]
        Confirmada = 2,
        [Display(Name = "Check-in Realizado")]
        CheckInRealizado = 3,
        [Display(Name = "Check-out Realizado")]
        CheckOutRealizado = 4,
        [Display(Name = "Cancelada")]
        Cancelada = 5,
        [Display(Name = "No Show")]
        NoShow = 6
    }

    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        // Dados do Hóspede
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome do Hóspede")]
        public string NomeHospede { get; set; } = string.Empty;

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo 100 caracteres")]
        [Display(Name = "Sobrenome do Hóspede")]
        public string SobrenomeHospede { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(150, ErrorMessage = "O email deve ter no máximo 150 caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; } = string.Empty;

        // Dados da Reserva
        [Required(ErrorMessage = "A data de check-in é obrigatória")]
        [Display(Name = "Data de Check-in")]
        [DataType(DataType.Date)]
        public DateTime DataCheckIn { get; set; }

        [Required(ErrorMessage = "A data de check-out é obrigatória")]
        [Display(Name = "Data de Check-out")]
        [DataType(DataType.Date)]
        public DateTime DataCheckOut { get; set; }

        [Display(Name = "Número de Hóspedes")]
        [Range(1, 10, ErrorMessage = "O número de hóspedes deve ser entre 1 e 10")]
        public int NumeroHospedes { get; set; } = 1;

        [StringLength(1000, ErrorMessage = "Os pedidos especiais devem ter no máximo 1000 caracteres")]
        [Display(Name = "Pedidos Especiais")]
        [DataType(DataType.MultilineText)]
        public string? PedidosEspeciais { get; set; }

        [Display(Name = "Status da Reserva")]
        public StatusReserva Status { get; set; } = StatusReserva.Pendente;

        [Display(Name = "Valor Total")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ValorTotal { get; set; }

        [Display(Name = "Data da Reserva")]
        public DateTime DataReserva { get; set; } = DateTime.Now;

        [Display(Name = "Data de Check-in Real")]
        public DateTime? DataCheckInReal { get; set; }

        [Display(Name = "Data de Check-out Real")]
        public DateTime? DataCheckOutReal { get; set; }

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres")]
        [Display(Name = "Observações")]
        [DataType(DataType.MultilineText)]
        public string? Observacoes { get; set; }

        // Chaves Estrangeiras
        [Required(ErrorMessage = "A acomodação é obrigatória")]
        [Display(Name = "Acomodação")]
        public int AcomodacaoId { get; set; }

        [Required(ErrorMessage = "O país é obrigatório")]
        [Display(Name = "País do Hóspede")]
        public int PaisId { get; set; }

        [Display(Name = "Usuário")]
        public string? UserId { get; set; }

        // Relacionamentos
        [ForeignKey("AcomodacaoId")]
        public virtual Acomodacao? Acomodacao { get; set; }

        [ForeignKey("PaisId")]
        public virtual Pais? Pais { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

        // Propriedades Calculadas
        [Display(Name = "Nome Completo")]
        public string NomeCompleto => $"{NomeHospede} {SobrenomeHospede}";

        [Display(Name = "Quantidade de Noites")]
        public int QuantidadeNoites => (DataCheckOut - DataCheckIn).Days;
    }
}
