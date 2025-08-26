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

        // Compat: GET /Reservas/DeleteConfirmed/{id} -> redireciona para a tela de confirmação padrão
        [HttpGet("Reservas/DeleteConfirmed/{id:int}")]
        [Authorize]
        public IActionResult DeleteConfirmedGet(int id)
        {
            return RedirectToAction("Delete", new { id });
        }

        // Compat: POST /Reservas/DeleteConfirmed/{id} -> executa a exclusão
        [HttpPost("Reservas/DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmedRoute(int id)
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

            if (User.IsInRole("Hospede"))
                return RedirectToAction("MinhasReservas");
            return RedirectToAction(nameof(Index));
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
        public IActionResult Create(int? id)
        {
            var acomodacoes = _context.Acomodacoes.Where(a => a.Ativa).ToList();
            ViewBag.AcomodacaoId = new SelectList(acomodacoes, "Id", "Nome", id);
            ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nome");
                ViewBag.Acomodacoes = acomodacoes;

            var reserva = new Reserva();
            if (id.HasValue)
                reserva.AcomodacaoId = id.Value;

            return View(reserva);
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Recepcionista,Hospede")]
        public async Task<IActionResult> Create([Bind("NomeHospede,SobrenomeHospede,Email,Telefone,DataCheckIn,DataCheckOut,NumeroHospedes,PedidosEspeciais,AcomodacaoId,PaisId")] Reserva reserva)
        {
            // Validação do limite de hóspedes
            var acomodacaoLimite = await _context.Acomodacoes.FindAsync(reserva.AcomodacaoId);
            if (acomodacaoLimite != null && acomodacaoLimite.QuantidadeMaximaHospedes > 0 && reserva.NumeroHospedes > acomodacaoLimite.QuantidadeMaximaHospedes)
            {
                ModelState.AddModelError("NumeroHospedes", $"A acomodação selecionada suporta no máximo {acomodacaoLimite.QuantidadeMaximaHospedes} hóspedes.");
            }

            // Validar datas
            if (reserva.DataCheckIn >= reserva.DataCheckOut)
            {
                ModelState.AddModelError("DataCheckOut", "A data de check-out deve ser posterior à data de check-in.");
            }
            else if (reserva.DataCheckIn < DateTime.Today)
            {
                ModelState.AddModelError("DataCheckIn", "A data de check-in não pode ser anterior à data atual.");
            }

            // Validação do mínimo de noites
            var acomodacaoMinNoites = await _context.Acomodacoes.FindAsync(reserva.AcomodacaoId);
            if (acomodacaoMinNoites != null && acomodacaoMinNoites.MinimoNoites > 0)
            {
                var quantidadeNoites = (reserva.DataCheckOut - reserva.DataCheckIn).Days;
                if (quantidadeNoites < acomodacaoMinNoites.MinimoNoites)
                {
                    ModelState.AddModelError("DataCheckOut", $"A acomodação exige reserva mínima de {acomodacaoMinNoites.MinimoNoites} noite(s).");
                }
            }

            // Só prossegue se não houver erros
            if (ModelState.IsValid)
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

            ViewBag.AcomodacaoId = new SelectList(_context.Acomodacoes.Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel), "Id", "Nome", reserva.AcomodacaoId);
            ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
            return View(reserva);
        }
        // GET: Reservas/MinhasReservas
    [Authorize(Roles = "Administrador,Hospede")]
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

            // Buscar datas ocupadas por acomodação (ignora a própria reserva)
            var reservas = _context.Reservas
                .Where(r => r.Status != StatusReserva.Cancelada && r.Id != reserva.Id)
                .Select(r => new
                {
                    r.AcomodacaoId,
                    r.DataCheckIn,
                    r.DataCheckOut
                })
                .ToList();

            var datasOcupadasPorAcomodacao = reservas
                .GroupBy(r => r.AcomodacaoId)
                .ToDictionary(g => g.Key, g => g.Select(r => new { inicio = r.DataCheckIn, fim = r.DataCheckOut }).ToList());

            ViewBag.DatasOcupadasPorAcomodacao = datasOcupadasPorAcomodacao;
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeHospede,SobrenomeHospede,Email,Telefone,DataCheckIn,DataCheckOut,NumeroHospedes,PedidosEspeciais,Status,DataCheckInReal,DataCheckOutReal,Observacoes,AcomodacaoId,PaisId")] Reserva reserva)
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
                // Validação de ordem das datas e data mínima (mesma regra do Create)
                if (reserva.DataCheckIn >= reserva.DataCheckOut)
                {
                    ModelState.AddModelError("DataCheckOut", "A data de check-out deve ser posterior à data de check-in.");
                }
                else if (reserva.DataCheckIn < DateTime.Today)
                {
                    ModelState.AddModelError("DataCheckIn", "A data de check-in não pode ser anterior à data atual.");
                }

                // Se houve erro nas datas, retorna para a view com os selects repovidados
                if (!ModelState.IsValid)
                {
                    ViewData["AcomodacaoId"] = new SelectList(_context.Acomodacoes.Where(a => a.Ativa), "Id", "Nome", reserva.AcomodacaoId);
                    ViewData["PaisId"] = new SelectList(_context.Paises, "Id", "Nome", reserva.PaisId);
                    return View(reserva);
                }

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
                    // Recalcular ValorTotal com base na acomodação e número de noites
                    var acomodacao = await _context.Acomodacoes.FindAsync(reserva.AcomodacaoId);
                    if (acomodacao != null)
                    {
                        var quantidadeNoites = (reserva.DataCheckOut - reserva.DataCheckIn).Days;
                        reservaOriginal.ValorTotal = acomodacao.Preco * quantidadeNoites;
                    }
                    // não sobrescrever DataReserva — manter a original
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

        // POST: CancelarReserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var reserva = await _context.Reservas.Include(r => r.Acomodacao).FirstOrDefaultAsync(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            // Permissão: hóspedes só podem cancelar suas próprias reservas
            if (User.IsInRole("Hospede"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (reserva.UserId != userId)
                {
                    return Forbid();
                }
            }

            // Marcar como cancelada
            reserva.Status = StatusReserva.Cancelada;

            // Se a acomodação estava marcada como ocupada por esta reserva, liberar
            if (reserva.Acomodacao != null && reserva.Acomodacao.Status == StatusAcomodacao.Ocupada)
            {
                reserva.Acomodacao.Status = StatusAcomodacao.Disponivel;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Reserva cancelada com sucesso.";

            if (User.IsInRole("Hospede"))
                return RedirectToAction("MinhasReservas");
            return RedirectToAction(nameof(Index));
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
