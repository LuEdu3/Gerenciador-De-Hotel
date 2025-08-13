using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.Controllers
{
    [AllowAnonymous]
    public class AcomodacoesController : Controller
    {
        // GET: Acomodacoes/Catalogo
        [AllowAnonymous]
        public async Task<IActionResult> Catalogo()
        {
            var acomodacoes = await _context.Acomodacoes
                .Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel)
                .ToListAsync();
            return View(acomodacoes);
        }
        private readonly ApplicationDbContext _context;

        public AcomodacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Acomodacoes
        public async Task<IActionResult> Index()
        {
            var acomodacoes = await _context.Acomodacoes
                .Include(a => a.AcomodacaoAmenidades)
                .ThenInclude(aa => aa.Amenidade)
                .ToListAsync();
            return View(acomodacoes);
        }

        // GET: Acomodacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acomodacao = await _context.Acomodacoes
                .Include(a => a.AcomodacaoAmenidades)
                .ThenInclude(aa => aa.Amenidade)
                .Include(a => a.Imagens)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (acomodacao == null)
            {
                return NotFound();
            }

            return View(acomodacao);
        }

        // GET: Acomodacoes/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Amenidades = _context.Amenidades.Where(a => a.Ativa).ToList();
            return View();
        }

        // POST: Acomodacoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,QuantidadeCamas,Preco,MinimoNoites,Status,ImagemPrincipalUrl,Ativa")] Acomodacao acomodacao)
        {
            if (ModelState.IsValid)
            {
                acomodacao.DataCriacao = DateTime.Now;
                _context.Add(acomodacao);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Acomodação criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Amenidades = _context.Amenidades.Where(a => a.Ativa).ToList();
            return View(acomodacao);
        }

        // GET: Acomodacoes/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acomodacao = await _context.Acomodacoes.FindAsync(id);
            if (acomodacao == null)
            {
                return NotFound();
            }
            ViewBag.Amenidades = _context.Amenidades.Where(a => a.Ativa).ToList();
            return View(acomodacao);
        }

        // POST: Acomodacoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,QuantidadeCamas,Preco,MinimoNoites,Status,ImagemPrincipalUrl,Ativa,DataCriacao")] Acomodacao acomodacao)
        {
            if (id != acomodacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    acomodacao.DataAtualizacao = DateTime.Now;
                    _context.Update(acomodacao);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Acomodação atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcomodacaoExists(acomodacao.Id))
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
            ViewBag.Amenidades = _context.Amenidades.Where(a => a.Ativa).ToList();
            return View(acomodacao);
        }

        // GET: Acomodacoes/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acomodacao = await _context.Acomodacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acomodacao == null)
            {
                return NotFound();
            }

            return View(acomodacao);
        }

        // POST: Acomodacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var acomodacao = await _context.Acomodacoes.FindAsync(id);
            if (acomodacao != null)
            {
                _context.Acomodacoes.Remove(acomodacao);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Acomodação excluída com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AcomodacaoExists(int id)
        {
            return _context.Acomodacoes.Any(e => e.Id == id);
        }
    }
}
