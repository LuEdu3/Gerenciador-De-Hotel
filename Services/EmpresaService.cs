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
        
        // Métodos para gerenciar fotos
        Task<List<EmpresaFoto>> GetTodasFotosAsync();
        Task<EmpresaFoto?> GetFotoPorIdAsync(int id);
        Task<bool> AdicionarFotoAsync(EmpresaFoto foto);
        Task<bool> AtualizarFotoAsync(EmpresaFoto foto);
        Task<bool> ExcluirFotoAsync(int id);
        Task<bool> ReordenarFotosAsync(List<int> idsOrdenados);
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
                // Bloquear criação se já existir alguma empresa (política: apenas uma empresa padrão)
                var existeQualquer = await _context.Empresas.AnyAsync();
                if (existeQualquer)
                {
                    return false;
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

        // Implementação dos métodos para gerenciar fotos
        public async Task<List<EmpresaFoto>> GetTodasFotosAsync()
        {
            var empresa = await GetEmpresaAtivaAsync();
            if (empresa == null) return new List<EmpresaFoto>();

            return await _context.EmpresaFotos
                .Where(f => f.EmpresaId == empresa.Id && f.Ativo)
                .OrderBy(f => f.Ordem)
                .ThenBy(f => f.Tipo)
                .ToListAsync();
        }

        public async Task<EmpresaFoto?> GetFotoPorIdAsync(int id)
        {
            return await _context.EmpresaFotos
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<bool> AdicionarFotoAsync(EmpresaFoto foto)
        {
            try
            {
                var empresa = await GetEmpresaAtivaAsync();
                if (empresa == null) return false;

                foto.EmpresaId = empresa.Id;
                foto.DataCriacao = DateTime.Now;
                foto.Ativo = true;

                // Se não foi especificada uma ordem, colocar no final
                if (foto.Ordem == 0)
                {
                    var ultimaOrdem = await _context.EmpresaFotos
                        .Where(f => f.EmpresaId == empresa.Id && f.Tipo == foto.Tipo)
                        .MaxAsync(f => (int?)f.Ordem) ?? 0;
                    foto.Ordem = ultimaOrdem + 1;
                }

                _context.EmpresaFotos.Add(foto);
                var resultado = await _context.SaveChangesAsync();

                return resultado > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao adicionar foto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AtualizarFotoAsync(EmpresaFoto foto)
        {
            try
            {
                var fotoExistente = await _context.EmpresaFotos
                    .FirstOrDefaultAsync(f => f.Id == foto.Id);

                if (fotoExistente == null) return false;

                fotoExistente.FotoUrl = foto.FotoUrl;
                fotoExistente.Descricao = foto.Descricao;
                fotoExistente.AltText = foto.AltText;
                fotoExistente.Ordem = foto.Ordem;
                fotoExistente.Tipo = foto.Tipo;
                fotoExistente.Ativo = foto.Ativo;

                _context.Entry(fotoExistente).State = EntityState.Modified;
                var resultado = await _context.SaveChangesAsync();

                return resultado > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao atualizar foto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExcluirFotoAsync(int id)
        {
            try
            {
                var foto = await _context.EmpresaFotos
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (foto == null) return false;

                // Exclusão lógica - marcar como inativa
                foto.Ativo = false;

                _context.Entry(foto).State = EntityState.Modified;
                var resultado = await _context.SaveChangesAsync();

                return resultado > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao excluir foto: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ReordenarFotosAsync(List<int> idsOrdenados)
        {
            try
            {
                var fotos = await _context.EmpresaFotos
                    .Where(f => idsOrdenados.Contains(f.Id))
                    .ToListAsync();

                for (int i = 0; i < idsOrdenados.Count; i++)
                {
                    var foto = fotos.FirstOrDefault(f => f.Id == idsOrdenados[i]);
                    if (foto != null)
                    {
                        foto.Ordem = i + 1;
                        _context.Entry(foto).State = EntityState.Modified;
                    }
                }

                var resultado = await _context.SaveChangesAsync();
                return resultado > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao reordenar fotos: {ex.Message}");
                return false;
            }
        }
    }
}
