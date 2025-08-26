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
        Task<bool> VerificarEmpresaExisteAsync(int id);
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
                // Buscar a empresa existente no banco com tracking
                var empresaExistente = await _context.Empresas
                    .Where(e => e.Id == empresa.Id)
                    .FirstOrDefaultAsync();
                
                if (empresaExistente == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Empresa com ID {empresa.Id} não encontrada");
                    return false;
                }

                // Atualizar apenas os campos necessários
                empresaExistente.Nome = empresa.Nome;
                empresaExistente.NomeResumido = empresa.NomeResumido;
                empresaExistente.LogoUrl = empresa.LogoUrl;
                empresaExistente.Slogan = empresa.Slogan;
                empresaExistente.DescricaoSobre = empresa.DescricaoSobre;
                empresaExistente.DescricaoBreve = empresa.DescricaoBreve;
                empresaExistente.AnoFundacao = empresa.AnoFundacao;
                empresaExistente.Telefone = empresa.Telefone;
                empresaExistente.WhatsApp = empresa.WhatsApp;
                empresaExistente.Email = empresa.Email;
                empresaExistente.Endereco = empresa.Endereco;
                empresaExistente.Cidade = empresa.Cidade;
                empresaExistente.Estado = empresa.Estado;
                empresaExistente.CEP = empresa.CEP;
                empresaExistente.Pais = empresa.Pais;
                empresaExistente.Website = empresa.Website;
                empresaExistente.Facebook = empresa.Facebook;
                empresaExistente.Instagram = empresa.Instagram;
                empresaExistente.Twitter = empresa.Twitter;
                empresaExistente.LinkedIn = empresa.LinkedIn;
                empresaExistente.HorarioCheckin = empresa.HorarioCheckin;
                empresaExistente.HorarioCheckout = empresa.HorarioCheckout;
                empresaExistente.DataAtualizacao = DateTime.Now;

                // Marcar como modificado explicitamente
                _context.Entry(empresaExistente).State = EntityState.Modified;
                
                var resultado = await _context.SaveChangesAsync();
                
                System.Diagnostics.Debug.WriteLine($"Resultado SaveChanges: {resultado} registros afetados");
                
                // Limpar cache
                _empresaCache = null;
                
                return resultado > 0;
            }
            catch (Exception ex)
            {
                // Log do erro para debugging
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar empresa: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
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

        public async Task<bool> VerificarEmpresaExisteAsync(int id)
        {
            return await _context.Empresas.AnyAsync(e => e.Id == id);
        }
    }
}
