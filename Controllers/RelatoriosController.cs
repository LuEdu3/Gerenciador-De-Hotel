using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GerenciadorHotel.Models;
using System.Linq;
using GerenciadorHotel.Data; 

namespace GerenciadorHotel.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RelatoriosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Ocupacao(int? dias)
        {
            int filtroDias = dias ?? 30;
            var dataLimite = DateTime.Now.AddDays(-filtroDias);
            var totalAcomodacoes = _context.Acomodacoes.Count();
            var reservasFiltradas = _context.Reservas
                .Where(r => (r.Status == StatusReserva.Confirmada || r.Status == StatusReserva.CheckInRealizado)
                    && r.DataCheckIn >= dataLimite)
                .ToList();

            var acomodacoes = _context.Acomodacoes.ToList();
            var ocupacaoPorQuarto = acomodacoes.Select(a => {
                var reservasQuarto = reservasFiltradas.Where(r => r.AcomodacaoId == a.Id).ToList();
                return new RelatorioOcupacaoPorQuartoViewModel
                {
                    AcomodacaoId = a.Id,
                    NomeAcomodacao = a.Nome,
                    TotalReservas = reservasQuarto.Count,
                    Reservas = reservasQuarto
                };
            }).ToList();

            var quartosOcupados = ocupacaoPorQuarto.Count(q => q.TotalReservas > 0);
            var modelo = new RelatorioOcupacaoViewModel
            {
                TotalAcomodacoes = totalAcomodacoes,
                QuartosOcupados = quartosOcupados,
                QuartosDisponiveis = totalAcomodacoes - quartosOcupados,
                TaxaOcupacao = totalAcomodacoes > 0 ? (double)quartosOcupados / totalAcomodacoes * 100 : 0,
                ReservasAtivas = reservasFiltradas,
                OcupacaoPorQuarto = ocupacaoPorQuarto,
                FiltroDias = filtroDias
            };

            return View(modelo);
        }

        public IActionResult Financeiro()
        {
            var reservasPagas = _context.Reservas
                .Where(r => r.Status == StatusReserva.Confirmada || r.Status == StatusReserva.CheckInRealizado)
                .ToList();

            var totalReceita = reservasPagas.Sum(r => r.ValorTotal);
            var totalReservas = reservasPagas.Count;

            var modelo = new RelatorioFinanceiroViewModel
            {
                TotalReceita = totalReceita,
                TotalReservas = totalReservas,
                Reservas = reservasPagas
            };

            return View(modelo);
        }
    }
}