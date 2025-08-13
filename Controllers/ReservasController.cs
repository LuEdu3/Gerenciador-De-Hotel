using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace GerenciadorHotel.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        // GET: Minhas Reservas (para hóspedes)
        [Authorize(Roles = "Hospede")]
        public async Task<IActionResult> MinhasReservas()
        {
            var userId = _userManager.GetUserId(User);
            var reservas = await _context.Reservas
                .Include(r => r.Acomodacao)
                .ThenInclude(a => a!.Imagens)
                .Include(r => r.Pais)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DataReserva)
                .ToListAsync();

            return View(reservas);
        }

        // POST: Cancelar Reserva (para hóspedes)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Hospede")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var userId = _userManager.GetUserId(User);
            var reserva = await _context.Reservas
                .Include(r => r.Acomodacao)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.Status != StatusReserva.Pendente)
            {
                TempData["ErrorMessage"] = "Só é possível cancelar reservas com status 'Pendente'.";
                return RedirectToAction(nameof(MinhasReservas));
            }

            // Verificar se a data de check-in não é hoje ou já passou
            if (reserva.DataCheckIn <= DateTime.Today)
            {
                TempData["ErrorMessage"] = "Não é possível cancelar reservas com check-in para hoje ou que já passaram.";
                return RedirectToAction(nameof(MinhasReservas));
            }

            reserva.Status = StatusReserva.Cancelada;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Reserva cancelada com sucesso!";
            return RedirectToAction(nameof(MinhasReservas));
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

                        // Associar a reserva ao usuário logado se for um hóspede
                        if (User.IsInRole("Hospede"))
                        {
                            reserva.UserId = _userManager.GetUserId(User);
                        }

                        _context.Add(reserva);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Reserva criada com sucesso!";

                        // Redirecionar baseado no role do usuário
                        if (User.IsInRole("Hospede"))
                        {
                            return RedirectToAction(nameof(MinhasReservas));
                        }
                        else
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AcomodacaoId", "Acomodação não encontrada.");
                    }
                }
            }

            // Se chegou até aqui, há erros no formulário
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
                if (reserva.Acomodacao != null)
                {
                    reserva.Acomodacao.Status = StatusAcomodacao.Ocupada;
                }

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
                if (reserva.Acomodacao != null)
                {
                    reserva.Acomodacao.Status = StatusAcomodacao.Disponivel;
                }

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
