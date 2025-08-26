using Microsoft.AspNetCore.Mvc;
using GerenciadorHotel.Services;

namespace GerenciadorHotel.ViewComponents
{
    public class EmpresaInfoViewComponent : ViewComponent
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaInfoViewComponent(IEmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var empresa = await _empresaService.GetEmpresaAtivaAsync();
            return View(empresa);
        }
    }
}
