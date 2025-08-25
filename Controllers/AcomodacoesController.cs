using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.Controllers
{
    [AllowAnonymous]
    public class AcomodacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcomodacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Acomodacoes/Catalogo
        [AllowAnonymous]
        public async Task<IActionResult> Catalogo()
        {
            var acomodacoes = await _context.Acomodacoes
                .Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel)
                .ToListAsync();
            return View(acomodacoes);
        }

        // GET: Acomodacoes/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Amenidades = _context.Amenidades.Where(a => a.Ativa).ToList();
            return View();
        }

        // GET: Acomodacoes
        public async Task<IActionResult> Index()
        {
            var acomodacoes = await _context.Acomodacoes
                .Include(a => a.AcomodacaoAmenidades)
                .ThenInclude(aa => aa.Amenidade)
                .Include(a => a.Imagens)
                .ToListAsync();
            return View(acomodacoes);
        }

        // POST: Acomodacoes/Create
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,QuantidadeCamas,Preco,MinimoNoites,Status,Ativa")] Acomodacao acomodacao)
        {
            var files = Request.Form.Files;
            int principalIndex = 0;
            int.TryParse(Request.Form["ImagemPrincipalIndex"], out principalIndex);
            if (files == null || files.Count < 1 || files.Count > 10)
            {
                ModelState.AddModelError("Imagens", "Selecione entre 1 e 10 imagens.");
            }
            if (ModelState.IsValid)
            {
                acomodacao.DataCriacao = DateTime.Now;
                _context.Add(acomodacao);
                await _context.SaveChangesAsync();

                int imgIndex = 0;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Name != "Imagens") continue;
                        if (file.Length > 0)
                        {
                            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");
                            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                            var fileName = $"acomodacao_{acomodacao.Id}_{DateTime.Now.Ticks}_{imgIndex}{Path.GetExtension(file.FileName)}";
                            var filePath = Path.Combine(uploads, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var url = $"/imagens/{fileName}";
                            var imagem = new ImagemAcomodacao
                            {
                                AcomodacaoId = acomodacao.Id,
                                ImagemUrl = url,
                                Ordem = imgIndex,
                                Ativa = true,
                                DataUpload = DateTime.Now
                            };
                            _context.ImagensAcomodacao.Add(imagem);
                            if (imgIndex == principalIndex)
                            {
                                acomodacao.ImagemPrincipalUrl = url;
                            }
                            imgIndex++;
                        }
                    }
                }

                // Processar URLs adicionais enviadas nos campos ImagensAcomodacao[0..n]
                // Permite que o usuário cole URLs no formulário (carousel de imagens)
                try
                {
                    // coletar entradas com índice até 20 (seguro e extensível)
                    for (int i = 0; i < 20; i++)
                    {
                        var key = $"ImagensAcomodacao[{i}]";
                        if (!Request.Form.ContainsKey(key)) continue;
                        var urlValue = Request.Form[key].ToString()?.Trim();
                        if (string.IsNullOrEmpty(urlValue)) continue;
                        if (urlValue.Length > 255) urlValue = urlValue.Substring(0, 255);

                        var imagemUrl = urlValue;
                        var imagem = new ImagemAcomodacao
                        {
                            AcomodacaoId = acomodacao.Id,
                            ImagemUrl = imagemUrl,
                            Ordem = imgIndex,
                            Ativa = true,
                            DataUpload = DateTime.Now
                        };
                        _context.ImagensAcomodacao.Add(imagem);

                        // Se ainda não existe imagem principal, e a URL foi informada, usar como principal
                        if (string.IsNullOrEmpty(acomodacao.ImagemPrincipalUrl))
                        {
                            acomodacao.ImagemPrincipalUrl = imagemUrl;
                        }

                        imgIndex++;
                    }
                }
                catch
                {
                    // Não falhar o fluxo se algo der errado ao processar urls; apenas ignora.
                }
                await _context.SaveChangesAsync();
                // Processar amenidades selecionadas (form usa checkboxes name="AmenidadesSelecionadas" com valores contendo os IDs)
                try
                {
                    var selected = Request.Form["AmenidadesSelecionadas"].ToArray();

                    if (selected != null && selected.Length > 0)
                    {
                        var ids = selected.Select(s => int.TryParse(s, out var id) ? id : 0).Where(i => i > 0).Distinct().ToList();
                        if (ids.Any())
                        {
                            var existentes = _context.AcomodacaoAmenidades.Where(aa => aa.AcomodacaoId == acomodacao.Id).Select(aa => aa.AmenidadeId).ToList();
                            // Adicionar novos
                            foreach (var id in ids)
                            {
                                if (!existentes.Contains(id))
                                {
                                    _context.AcomodacaoAmenidades.Add(new AcomodacaoAmenidade
                                    {
                                        AcomodacaoId = acomodacao.Id,
                                        AmenidadeId = id,
                                        DataAssociacao = DateTime.Now
                                    });
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch
                {
                    // Não falhar o fluxo se algo der errado ao processar amenidades
                }
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

            var acomodacao = await _context.Acomodacoes
                .Include(a => a.AcomodacaoAmenidades)
                .FirstOrDefaultAsync(a => a.Id == id);
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
                    // Processar uploads de arquivos (novas imagens enviadas no Edit)
                    var files = Request.Form.Files;
                    int nextIndex = _context.ImagensAcomodacao.Where(i => i.AcomodacaoId == acomodacao.Id).Count();
                    if (files != null && files.Count > 0)
                    {
                        int imgIndex = nextIndex;
                        foreach (var file in files)
                        {
                            if (file.Name != "Imagens") continue;
                            if (file.Length <= 0) continue;
                            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");
                            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                            var fileName = $"acomodacao_{acomodacao.Id}_{DateTime.Now.Ticks}_{imgIndex}{Path.GetExtension(file.FileName)}";
                            var filePath = Path.Combine(uploads, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            var url = $"/imagens/{fileName}";
                            var imagem = new ImagemAcomodacao
                            {
                                AcomodacaoId = acomodacao.Id,
                                ImagemUrl = url,
                                Ordem = imgIndex,
                                Ativa = true,
                                DataUpload = DateTime.Now
                            };
                            _context.ImagensAcomodacao.Add(imagem);
                            if (string.IsNullOrEmpty(acomodacao.ImagemPrincipalUrl))
                            {
                                acomodacao.ImagemPrincipalUrl = url;
                            }
                            imgIndex++;
                        }
                        nextIndex = imgIndex;
                    }

                    // Em seguida processar quaisquer URLs adicionais ImagensAcomodacao[0..n]
                    for (int i = 0; i < 20; i++)
                    {
                        var key = $"ImagensAcomodacao[{i}]";
                        if (!Request.Form.ContainsKey(key)) continue;
                        var urlValue = Request.Form[key].ToString()?.Trim();
                        if (string.IsNullOrEmpty(urlValue)) continue;
                        if (urlValue.Length > 255) urlValue = urlValue.Substring(0, 255);

                        var imagem = new ImagemAcomodacao
                        {
                            AcomodacaoId = acomodacao.Id,
                            ImagemUrl = urlValue,
                            Ordem = nextIndex,
                            Ativa = true,
                            DataUpload = DateTime.Now
                        };
                        _context.ImagensAcomodacao.Add(imagem);
                        if (string.IsNullOrEmpty(acomodacao.ImagemPrincipalUrl))
                        {
                            acomodacao.ImagemPrincipalUrl = urlValue;
                        }
                        nextIndex++;
                    }

                    acomodacao.DataAtualizacao = DateTime.Now;
                    _context.Update(acomodacao);
                    await _context.SaveChangesAsync();
                    // Atualizar amenidades associadas com base na seleção do formulário
                    try
                    {
                        var selected = Request.Form["AmenidadesSelecionadas"].ToArray();

                        var ids = selected.Select(s => int.TryParse(s, out var id) ? id : 0).Where(i => i > 0).Distinct().ToList();

                        // Se não houver seleção, remover todas as associações
                        if (ids == null || !ids.Any())
                        {
                            var existentes = _context.AcomodacaoAmenidades.Where(aa => aa.AcomodacaoId == acomodacao.Id).ToList();
                            if (existentes.Any())
                            {
                                _context.AcomodacaoAmenidades.RemoveRange(existentes);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var existentesIds = _context.AcomodacaoAmenidades.Where(aa => aa.AcomodacaoId == acomodacao.Id).Select(aa => aa.AmenidadeId).ToList();
                            // Remover associações que não estão na lista desejada
                            var existentes2 = _context.AcomodacaoAmenidades.Where(aa => aa.AcomodacaoId == acomodacao.Id).ToList();
                            foreach (var ex in existentes2)
                            {
                                if (!ids.Contains(ex.AmenidadeId))
                                {
                                    _context.AcomodacaoAmenidades.Remove(ex);
                                }
                            }

                            // Adicionar novas associações
                            foreach (var amenId in ids)
                            {
                                if (!existentesIds.Contains(amenId))
                                {
                                    _context.AcomodacaoAmenidades.Add(new AcomodacaoAmenidade
                                    {
                                        AcomodacaoId = acomodacao.Id,
                                        AmenidadeId = amenId,
                                        DataAssociacao = DateTime.Now
                                    });
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                    catch
                    {
                        // Ignorar falhas na atualização de amenidades
                    }
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

        // GET: Acomodacoes/Details/5
        [AllowAnonymous]
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
                .FirstOrDefaultAsync(a => a.Id == id);

            if (acomodacao == null)
            {
                return NotFound();
            }

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
