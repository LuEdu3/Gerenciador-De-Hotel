using System.Collections.Generic;

namespace GerenciadorHotel.Models
{
    public class RelatorioFinanceiroViewModel
    {
        public decimal TotalReceita { get; set; }
        public int TotalReservas { get; set; }
    public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}