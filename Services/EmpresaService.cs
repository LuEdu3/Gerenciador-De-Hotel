using GerenciadorHotel.Data;
using GerenciadorHotel.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorHotel.Services
{
    public interface IEmpresaService
    {
        Task<Empresa?> GetEmpresaAtivaAsync();
        Task<Empresa?> GetEmpresaComDetalhesAsync();
        Task<List<EmpresaFoto>> GetFotosPorTipoAsync(TipoFotoEmpresa tipo);
        Task<List<EmpresaServico>> GetServicosAtivosAsync();
        Task<List<EmpresaPremio>> GetPremiosAtivosAsync();
        Task<bool> AtualizarEmpresaAsync(Empresa empresa);
        Task<bool> CriarEmpresaAsync(Empresa empresa);
    }

    public class EmpresaService : IEmpresaService
    {
        private readonly ApplicationDbContext _context;
        private static Empresa? _empresaCache;
        private static DateTime _ultimaAtualizacaoCache = DateTime.MinValue;
        private static readonly TimeSpan _tempoCache = TimeSpan.FromMinutes(30);

        public EmpresaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Empresa?> GetEmpresaAtivaAsync()
        {
            // Verificar cache
            if (_empresaCache != null && DateTime.Now - _ultimaAtualizacaoCache < _tempoCache)
            {
                return _empresaCache;
            }

            // Buscar no banco
            _empresaCache = await _context.Empresas
                .Where(e => e.Ativo)
                .FirstOrDefaultAsync();

            _ultimaAtualizacaoCache = DateTime.Now;
            return _empresaCache;
        }

        public async Task<Empresa?> GetEmpresaComDetalhesAsync()
        {
            return await _context.Empresas
                .Include(e => e.Fotos!.Where(f => f.Ativo))
                .Include(e => e.Servicos!.Where(s => s.Ativo))
                .Include(e => e.Premios!.Where(p => p.Ativo))
                .Where(e => e.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task<List<EmpresaFoto>> GetFotosPorTipoAsync(TipoFotoEmpresa tipo)
        {
            var empresa = await GetEmpresaAtivaAsync();
            if (empresa == null) return new List<EmpresaFoto>();

            return await _context.EmpresaFotos
                .Where(f => f.EmpresaId == empresa.Id && f.Tipo == tipo && f.Ativo)
                .OrderBy(f => f.Ordem)
                .ToListAsync();
        }

        public async Task<List<EmpresaServico>> GetServicosAtivosAsync()
        {
            var empresa = await GetEmpresaAtivaAsync();
            if (empresa == null) return new List<EmpresaServico>();

            return await _context.EmpresaServicos
                .Where(s => s.EmpresaId == empresa.Id && s.Ativo)
                .OrderBy(s => s.Ordem)
                .ToListAsync();
        }

        public async Task<List<EmpresaPremio>> GetPremiosAtivosAsync()
        {
            var empresa = await GetEmpresaAtivaAsync();
            if (empresa == null) return new List<EmpresaPremio>();

            return await _context.EmpresaPremios
                .Where(p => p.EmpresaId == empresa.Id && p.Ativo)
                .OrderBy(p => p.Ordem)
                .ThenByDescending(p => p.Ano)
                .ToListAsync();
        }

        public async Task<bool> AtualizarEmpresaAsync(Empresa empresa)
        {
            try
            {
                empresa.DataAtualizacao = DateTime.Now;
                _context.Empresas.Update(empresa);
                await _context.SaveChangesAsync();
                
                // Limpar cache
                _empresaCache = null;
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CriarEmpresaAsync(Empresa empresa)
        {
            try
            {
                // Desativar outras empresas se existirem
                var empresasExistentes = await _context.Empresas.Where(e => e.Ativo).ToListAsync();
                foreach (var emp in empresasExistentes)
                {
                    emp.Ativo = false;
                    emp.DataAtualizacao = DateTime.Now;
                }

                empresa.Ativo = true;
                empresa.DataCriacao = DateTime.Now;
                
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
                
                // Limpar cache
                _empresaCache = null;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
