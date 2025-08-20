
using GerenciadorHotel.Models;
using System.Collections.Generic;

namespace GerenciadorHotel.ViewModels
{
    public class RelatorioOcupacaoViewModel
    {
        public int TotalAcomodacoes { get; set; }
        public int QuartosOcupados { get; set; }
        public int QuartosDisponiveis { get; set; }
        public double TaxaOcupacao { get; set; }
        public List<Reserva>? ReservasAtivas { get; set; }
        public List<RelatorioOcupacaoPorQuartoViewModel>? OcupacaoPorQuarto { get; set; }
        public int FiltroDias { get; set; }
    }

    public class RelatorioOcupacaoPorQuartoViewModel
    {
        public int AcomodacaoId { get; set; }
        public string? NomeAcomodacao { get; set; }
        public int TotalReservas { get; set; }
        public List<Reserva>? Reservas { get; set; }
    }
}