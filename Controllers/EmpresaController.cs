using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GerenciadorHotel.Services;
using GerenciadorHotel.ViewModels;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EmpresaController : Controller
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaController(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        // GET: Empresa
        public async Task<IActionResult> Index()
        {
            var empresa = await _empresaService.GetEmpresaComDetalhesAsync();
            
            if (empresa == null)
            {
                // Se não existe empresa, redirecionar para criar
                return RedirectToAction(nameof(Create));
            }

            var viewModel = EmpresaViewModel.FromEmpresa(empresa);
            return View(viewModel);
        }

        // GET: Empresa/Create
        public IActionResult Create()
        {
            var viewModel = new EmpresaViewModel();
            return View(viewModel);
        }

        // POST: Empresa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpresaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var empresa = viewModel.ToEmpresa();
            var sucesso = await _empresaService.CriarEmpresaAsync(empresa);

            if (sucesso)
            {
                TempData["Success"] = "Empresa criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Erro ao criar empresa. Tente novamente.";
            return View(viewModel);
        }

        // GET: Empresa/Edit
        public async Task<IActionResult> Edit()
        {
            var empresa = await _empresaService.GetEmpresaComDetalhesAsync();
            
            if (empresa == null)
            {
                return RedirectToAction(nameof(Create));
            }

            var viewModel = EmpresaViewModel.FromEmpresa(empresa);
            return View(viewModel);
        }

        // POST: Empresa/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmpresaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var empresa = viewModel.ToEmpresa();
            var sucesso = await _empresaService.AtualizarEmpresaAsync(empresa);

            if (sucesso)
            {
                TempData["Success"] = "Empresa atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Erro ao atualizar empresa. Tente novamente.";
            return View(viewModel);
        }

        // GET: Empresa/Fotos
        public async Task<IActionResult> Fotos()
        {
            var empresa = await _empresaService.GetEmpresaAtivaAsync();
            
            if (empresa == null)
            {
                return RedirectToAction(nameof(Create));
            }

            var fotos = await _empresaService.GetFotosPorTipoAsync(TipoFotoEmpresa.Galeria);
            ViewBag.EmpresaId = empresa.Id;
            ViewBag.EmpresaNome = empresa.Nome;
            
            return View(fotos);
        }

        // GET: Empresa/Servicos
        public async Task<IActionResult> Servicos()
        {
            var empresa = await _empresaService.GetEmpresaAtivaAsync();
            
            if (empresa == null)
            {
                return RedirectToAction(nameof(Create));
            }

            var servicos = await _empresaService.GetServicosAtivosAsync();
            ViewBag.EmpresaId = empresa.Id;
            ViewBag.EmpresaNome = empresa.Nome;
            
            return View(servicos);
        }

        // GET: Empresa/Premios
        public async Task<IActionResult> Premios()
        {
            var empresa = await _empresaService.GetEmpresaAtivaAsync();
            
            if (empresa == null)
            {
                return RedirectToAction(nameof(Create));
            }

            var premios = await _empresaService.GetPremiosAtivosAsync();
            ViewBag.EmpresaId = empresa.Id;
            ViewBag.EmpresaNome = empresa.Nome;
            
            return View(premios);
        }

        // API para obter dados da empresa (usado nas views públicas)
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetDadosEmpresa()
        {
            var empresa = await _empresaService.GetEmpresaAtivaAsync();
            
            if (empresa == null)
            {
                return Json(new { success = false, message = "Empresa não encontrada" });
            }

            return Json(new { 
                success = true, 
                data = new {
                    nome = empresa.Nome,
                    nomeResumido = empresa.NomeResumido,
                    logoUrl = empresa.LogoUrl,
                    slogan = empresa.Slogan,
                    descricaoBreve = empresa.DescricaoBreve,
                    descricaoSobre = empresa.DescricaoSobre,
                    anoFundacao = empresa.AnoFundacao,
                    telefone = empresa.Telefone,
                    whatsapp = empresa.WhatsApp,
                    email = empresa.Email,
                    endereco = empresa.Endereco,
                    cidade = empresa.Cidade,
                    estado = empresa.Estado,
                    cep = empresa.CEP,
                    pais = empresa.Pais,
                    website = empresa.Website,
                    facebook = empresa.Facebook,
                    instagram = empresa.Instagram,
                    twitter = empresa.Twitter,
                    linkedin = empresa.LinkedIn,
                    horarioCheckin = empresa.HorarioCheckin,
                    horarioCheckout = empresa.HorarioCheckout
                }
            });
        }

        // GET: Empresa/Sobre - Página pública sobre a empresa
        [AllowAnonymous]
        public async Task<IActionResult> Sobre()
        {
            var empresa = await _empresaService.GetEmpresaComDetalhesAsync();
            
            if (empresa == null)
            {
                return NotFound("Informações da empresa não encontradas.");
            }

            // Buscar fotos da galeria
            var fotos = await _empresaService.GetFotosPorTipoAsync(TipoFotoEmpresa.Galeria);
            
            var viewModel = EmpresaViewModel.FromEmpresa(empresa);
            ViewBag.FotosGaleria = fotos;
            
            return View(viewModel);
        }
    }
}
