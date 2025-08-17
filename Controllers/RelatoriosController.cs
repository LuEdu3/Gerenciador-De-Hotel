using Microsoft.AspNetCore.Mvc;
using GerenciadorHotel.Models;
using System.Linq;
using GerenciadorHotel.Data; // Adicione este using

namespace GerenciadorHotel.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Ocupacao()
        {
            var totalAcomodacoes = _context.Acomodacoes.Count();
            var reservasAtivas = _context.Reservas
                .Where(r => r.Status == StatusReserva.Confirmada || r.Status == StatusReserva.CheckInRealizado)
                .ToList();

            var quartosOcupados = reservasAtivas.Select(r => r.AcomodacaoId).Distinct().Count();
            var quartosDisponiveis = totalAcomodacoes - quartosOcupados;
            var taxaOcupacao = totalAcomodacoes > 0 ? (double)quartosOcupados / totalAcomodacoes * 100 : 0;

            var modelo = new RelatorioOcupacaoViewModel
            {
                TotalAcomodacoes = totalAcomodacoes,
                QuartosOcupados = quartosOcupados,
                QuartosDisponiveis = quartosDisponiveis,
                TaxaOcupacao = taxaOcupacao,
                ReservasAtivas = reservasAtivas
            };

            return View(modelo);
        }
    }
}