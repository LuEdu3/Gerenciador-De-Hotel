using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GerenciadorHotel.Models;
using GerenciadorHotel.Data;
using System;
using System.Linq;

namespace Gerenciador_De_Hotel.Controllers
{
    [Authorize(Roles = "Recepcionista")]
    public class CheckInController : Controller
    {
    private readonly ApplicationDbContext _context;

        public CheckInController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hoje = DateTime.Today;
            var reservasHoje = _context.Reservas
                .Where(r => r.DataCheckIn.Date == hoje)
                .ToList();
            return View(reservasHoje);
        }

        [HttpPost]
        public IActionResult ConfirmarCheckin(int id)
        {
            var reserva = _context.Reservas.FirstOrDefault(r => r.Id == id);
            if (reserva != null)
            {
                reserva.Status = StatusReserva.CheckInRealizado;
                reserva.DataCheckInReal = DateTime.Now;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
