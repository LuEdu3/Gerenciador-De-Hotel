using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorHotel.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

    // GET: Reservas
    [Authorize] // Permite qualquer usuário autenticado
    public async Task<IActionResult> Index()
        {
            // Se hóspede tentar acessar, redireciona para suas próprias reservas
            if (User.IsInRole("Hospede"))
            {
                return RedirectToAction("MinhasReservas");
            }

            // Apenas admins e recepcionistas chegam aqui
            if (!User.IsInRole("Administrador") && !User.IsInRole("Recepcionista"))
            {
                return Forbid();
            }

            var reservas = await _context.Reservas
                .Include(r => r.Acomodacao)
                .Include(r => r.Pais)
                .OrderByDescending(r => r.DataReserva)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Details/5
        [Authorize]
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

            // Se for hóspede, só pode acessar detalhes da própria reserva
            if (User.IsInRole("Hospede"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (reserva.UserId != userId)
                {
                    return Forbid();
                }
            }

            return View(reserva);
        }

    // GET: Reservas/Create
    [Authorize(Roles = "Administrador,Recepcionista,Hospede")]
    public IActionResult Create()
        {
            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel), "Id", "Nome");
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome");
            return View();
        }

    // POST: Reservas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador,Recepcionista,Hospede")]
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
                    // Verificar conflitos de reserva sobreposta
                    var conflito = await _context.Reservas
                        .Where(r => r.AcomodacaoId == reserva.AcomodacaoId && r.Status != StatusReserva.Cancelada)
                        .Where(r =>
                            (reserva.DataCheckIn < r.DataCheckOut && reserva.DataCheckOut > r.DataCheckIn)
                        )
                        .AnyAsync();
                    if (conflito)
                    {
                        ModelState.AddModelError("AcomodacaoId", "Já existe uma reserva para este quarto no período selecionado. Escolha outra acomodação ou datas.");
                    }
                    else
                    {
                        var acomodacao = await _context.Acomodacoes.FindAsync(reserva.AcomodacaoId);
                        if (acomodacao != null)
                        {
                            // Calcular valor total
                            var quantidadeNoites = (reserva.DataCheckOut - reserva.DataCheckIn).Days;
                            reserva.ValorTotal = acomodacao.Preco * quantidadeNoites;
                            reserva.DataReserva = DateTime.Now;
                            reserva.Status = StatusReserva.Pendente;
                            // Associar ao usuário logado
                            if (User.Identity != null && User.Identity.IsAuthenticated)
                            {
                                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                                reserva.UserId = userId;
                            }

                            _context.Add(reserva);
                            await _context.SaveChangesAsync();
                            TempData["SuccessMessage"] = "Reserva criada com sucesso!";
                            return RedirectToAction("MinhasReservas");
                        }
                    }
                }
            }

            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel), "Id", "Nome", reserva.AcomodacaoId);
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
            return View(reserva);
        }
        // GET: Reservas/MinhasReservas
        [Authorize(Roles = "Administrador,Recepcionista,Hospede")]
        public async Task<IActionResult> MinhasReservas()
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var reservas = await _context.Reservas
                .Include(r => r.Acomodacao)
                .Include(r => r.Pais)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DataReserva)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Edit/5
        [Authorize]
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

            // Se for hóspede, só pode editar a própria reserva
            if (User.IsInRole("Hospede"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (reserva.UserId != userId)
                {
                    return Forbid();
                }
            }

            ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa), "Id", "Nome", reserva.AcomodacaoId);
            ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeHospede,SobrenomeHospede,Email,Telefone,DataCheckIn,DataCheckOut,NumeroHospedes,PedidosEspeciais,Status,ValorTotal,DataReserva,DataCheckInReal,DataCheckOutReal,Observacoes,AcomodacaoId,PaisId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            var reservaOriginal = await _context.Reservas.FindAsync(id);
            if (reservaOriginal == null)
            {
                return NotFound();
            }

            // Se for hóspede, só pode editar a própria reserva
            if (User.IsInRole("Hospede"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (reservaOriginal.UserId != userId)
                {
                    return Forbid();
                }
            }

            if (ModelState.IsValid)
            {
                // Validação de conflito de datas (ignora a própria reserva)
                var conflito = await _context.Reservas
                    .Where(r => r.AcomodacaoId == reserva.AcomodacaoId && r.Id != reserva.Id && r.Status != StatusReserva.Cancelada)
                    .Where(r =>
                        (reserva.DataCheckIn < r.DataCheckOut && reserva.DataCheckOut > r.DataCheckIn)
                    )
                    .AnyAsync();
                if (conflito)
                {
                    ModelState.AddModelError("AcomodacaoId", "Já existe uma reserva para este quarto no período selecionado. Escolha outra acomodação ou datas.");
                }
                else
                {
                    // Atualiza apenas os campos permitidos
                    reservaOriginal.NomeHospede = reserva.NomeHospede;
                    reservaOriginal.SobrenomeHospede = reserva.SobrenomeHospede;
                    reservaOriginal.Email = reserva.Email;
                    reservaOriginal.Telefone = reserva.Telefone;
                    reservaOriginal.DataCheckIn = reserva.DataCheckIn;
                    reservaOriginal.DataCheckOut = reserva.DataCheckOut;
                    reservaOriginal.NumeroHospedes = reserva.NumeroHospedes;
                    reservaOriginal.PedidosEspeciais = reserva.PedidosEspeciais;
                    reservaOriginal.Status = reserva.Status;
                    reservaOriginal.ValorTotal = reserva.ValorTotal;
                    reservaOriginal.DataReserva = reserva.DataReserva;
                    reservaOriginal.DataCheckInReal = reserva.DataCheckInReal;
                    reservaOriginal.DataCheckOutReal = reserva.DataCheckOutReal;
                    reservaOriginal.Observacoes = reserva.Observacoes;
                    reservaOriginal.AcomodacaoId = reserva.AcomodacaoId;
                    reservaOriginal.PaisId = reserva.PaisId;

                    try
                    {
                        _context.Update(reservaOriginal);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Reserva atualizada com sucesso!";
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ReservaExists(reservaOriginal.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    // Redireciona para MinhasReservas se hóspede
                    if (User.IsInRole("Hospede"))
                        return RedirectToAction("MinhasReservas");
                    return RedirectToAction(nameof(Index));
                }
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
        [Authorize]
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

            // Se for hóspede, só pode excluir a própria reserva
            if (User.IsInRole("Hospede"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (reserva.UserId != userId)
                {
                    return Forbid();
                }
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            // Se for hóspede, só pode excluir a própria reserva
            if (User.IsInRole("Hospede"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (reserva.UserId != userId)
                {
                    return Forbid();
                }
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Reserva excluída com sucesso!";

            // Redireciona para MinhasReservas se hóspede
            if (User.IsInRole("Hospede"))
                return RedirectToAction("MinhasReservas");
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
