using System.Collections.Generic;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.Models
{
    public class RelatorioOcupacaoViewModel
    {
        public int TotalAcomodacoes { get; set; }
        public int QuartosOcupados { get; set; }
        public int QuartosDisponiveis { get; set; }
        public double TaxaOcupacao { get; set; }
        public List<Reserva> ReservasAtivas { get; set; }
    }
}