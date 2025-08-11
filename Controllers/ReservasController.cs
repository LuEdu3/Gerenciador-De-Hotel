using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorHotel.Controllers
{
    [Authorize(Roles = "Administrador,Recepcionista")]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Acomodacao)
                .Include(r => r.Pais)
                .OrderByDescending(r => r.DataReserva)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Acomodacao)
                .Include(r => r.Pais)
                .Include(r => r.Pagamentos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel), "Id", "Nome");
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome");
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeHospede,SobrenomeHospede,Email,Telefone,DataCheckIn,DataCheckOut,NumeroHospedes,PedidosEspeciais,AcomodacaoId,PaisId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                // Validar datas
                if (reserva.DataCheckIn >= reserva.DataCheckOut)
                {
                    ModelState.AddModelError("DataCheckOut", "A data de check-out deve ser posterior à data de check-in.");
                }
                else if (reserva.DataCheckIn < DateTime.Today)
                {
                    ModelState.AddModelError("DataCheckIn", "A data de check-in não pode ser anterior à data atual.");
                }
                else
                {
                    // Verificar disponibilidade
                    var acomodacao = await _context.Acomodacoes.FindAsync(reserva.AcomodacaoId);
                    if (acomodacao != null)
                    {
                        // Calcular valor total
                        var quantidadeNoites = (reserva.DataCheckOut - reserva.DataCheckIn).Days;
                        reserva.ValorTotal = acomodacao.Preco * quantidadeNoites;
                        reserva.DataReserva = DateTime.Now;
                        reserva.Status = StatusReserva.Pendente;

                        _context.Add(reserva);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Reserva criada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel), "Id", "Nome", reserva.AcomodacaoId);
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa), "Id", "Nome", reserva.AcomodacaoId);
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeHospede,SobrenomeHospede,Email,Telefone,DataCheckIn,DataCheckOut,NumeroHospedes,PedidosEspeciais,Status,ValorTotal,DataReserva,DataCheckInReal,DataCheckOutReal,Observacoes,AcomodacaoId,PaisId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Reserva atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa), "Id", "Nome", reserva.AcomodacaoId);
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
            return View(reserva);
        }

        // POST: CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id)
        {
            var reserva = await _context.Reservas.Include(r => r.Acomodacao).FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.Status == StatusReserva.Confirmada)
            {
                reserva.Status = StatusReserva.CheckInRealizado;
                reserva.DataCheckInReal = DateTime.Now;
                
                // Atualizar status da acomodação
                reserva.Acomodacao.Status = StatusAcomodacao.Ocupada;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Check-in realizado com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Check-in não pode ser realizado. Verifique o status da reserva.";
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        // POST: CheckOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var reserva = await _context.Reservas.Include(r => r.Acomodacao).FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.Status == StatusReserva.CheckInRealizado)
            {
                reserva.Status = StatusReserva.CheckOutRealizado;
                reserva.DataCheckOutReal = DateTime.Now;
                
                // Liberar acomodação
                reserva.Acomodacao.Status = StatusAcomodacao.Disponivel;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Check-out realizado com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Check-out não pode ser realizado. Verifique o status da reserva.";
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: Reservas/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Acomodacao)
                .Include(r => r.Pais)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva excluída com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
