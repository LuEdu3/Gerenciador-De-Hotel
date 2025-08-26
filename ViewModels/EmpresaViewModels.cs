using System.ComponentModel.DataAnnotations;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.ViewModels
{
    public class EmpresaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da empresa é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome da Empresa")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "O nome resumido deve ter no máximo 50 caracteres")]
        [Display(Name = "Nome Resumido")]
        public string? NomeResumido { get; set; }

        [StringLength(255, ErrorMessage = "A URL da logo deve ter no máximo 255 caracteres")]
        [Display(Name = "URL da Logo")]
        public string? LogoUrl { get; set; }

        [StringLength(500, ErrorMessage = "O slogan deve ter no máximo 500 caracteres")]
        [Display(Name = "Slogan")]
        public string? Slogan { get; set; }

        [StringLength(2000, ErrorMessage = "A descrição sobre deve ter no máximo 2000 caracteres")]
        [Display(Name = "Sobre a Empresa")]
        public string? DescricaoSobre { get; set; }

        [StringLength(1000, ErrorMessage = "A descrição breve deve ter no máximo 1000 caracteres")]
        [Display(Name = "Descrição Breve")]
        public string? DescricaoBreve { get; set; }

        [Range(1800, 2100, ErrorMessage = "O ano de fundação deve estar entre 1800 e 2100")]
        [Display(Name = "Ano de Fundação")]
        public int? AnoFundacao { get; set; }

        [StringLength(100, ErrorMessage = "O telefone deve ter no máximo 100 caracteres")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [StringLength(100, ErrorMessage = "O WhatsApp deve ter no máximo 100 caracteres")]
        [Display(Name = "WhatsApp")]
        public string? WhatsApp { get; set; }

        [StringLength(100, ErrorMessage = "O e-mail deve ter no máximo 100 caracteres")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres")]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres")]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [StringLength(50, ErrorMessage = "O estado deve ter no máximo 50 caracteres")]
        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [StringLength(20, ErrorMessage = "O CEP deve ter no máximo 20 caracteres")]
        [Display(Name = "CEP")]
        public string? CEP { get; set; }

        [StringLength(100, ErrorMessage = "O país deve ter no máximo 100 caracteres")]
        [Display(Name = "País")]
        public string? Pais { get; set; }

        [StringLength(100, ErrorMessage = "O website deve ter no máximo 100 caracteres")]
        [Url(ErrorMessage = "URL inválida")]
        [Display(Name = "Website")]
        public string? Website { get; set; }

        [StringLength(100, ErrorMessage = "O Facebook deve ter no máximo 100 caracteres")]
        [Display(Name = "Facebook")]
        public string? Facebook { get; set; }

        [StringLength(100, ErrorMessage = "O Instagram deve ter no máximo 100 caracteres")]
        [Display(Name = "Instagram")]
        public string? Instagram { get; set; }

        [StringLength(100, ErrorMessage = "O Twitter deve ter no máximo 100 caracteres")]
        [Display(Name = "Twitter")]
        public string? Twitter { get; set; }

        [StringLength(100, ErrorMessage = "O LinkedIn deve ter no máximo 100 caracteres")]
        [Display(Name = "LinkedIn")]
        public string? LinkedIn { get; set; }

        [StringLength(50, ErrorMessage = "O horário de check-in deve ter no máximo 50 caracteres")]
        [Display(Name = "Horário de Check-in")]
        public string? HorarioCheckin { get; set; }

        [StringLength(50, ErrorMessage = "O horário de check-out deve ter no máximo 50 caracteres")]
        [Display(Name = "Horário de Check-out")]
        public string? HorarioCheckout { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Propriedades de navegação para exibição
        public List<EmpresaFoto> Fotos { get; set; } = new();
        public List<EmpresaServico> Servicos { get; set; } = new();
        public List<EmpresaPremio> Premios { get; set; } = new();

        // Métodos auxiliares
        public static EmpresaViewModel FromEmpresa(Empresa empresa)
        {
            return new EmpresaViewModel
            {
                Id = empresa.Id,
                Nome = empresa.Nome ?? string.Empty,
                NomeResumido = empresa.NomeResumido,
                LogoUrl = empresa.LogoUrl,
                Slogan = empresa.Slogan,
                DescricaoSobre = empresa.DescricaoSobre,
                DescricaoBreve = empresa.DescricaoBreve,
                AnoFundacao = empresa.AnoFundacao,
                Telefone = empresa.Telefone,
                WhatsApp = empresa.WhatsApp,
                Email = empresa.Email,
                Endereco = empresa.Endereco,
                Cidade = empresa.Cidade,
                Estado = empresa.Estado,
                CEP = empresa.CEP,
                Pais = empresa.Pais,
                Website = empresa.Website,
                Facebook = empresa.Facebook,
                Instagram = empresa.Instagram,
                Twitter = empresa.Twitter,
                LinkedIn = empresa.LinkedIn,
                HorarioCheckin = empresa.HorarioCheckin,
                HorarioCheckout = empresa.HorarioCheckout,
                Ativo = empresa.Ativo,
                Fotos = empresa.Fotos?.ToList() ?? new List<EmpresaFoto>(),
                Servicos = empresa.Servicos?.ToList() ?? new List<EmpresaServico>(),
                Premios = empresa.Premios?.ToList() ?? new List<EmpresaPremio>()
            };
        }

        public Empresa ToEmpresa()
        {
            return new Empresa
            {
                Id = Id,
                Nome = Nome,
                NomeResumido = NomeResumido,
                LogoUrl = LogoUrl,
                Slogan = Slogan,
                DescricaoSobre = DescricaoSobre,
                DescricaoBreve = DescricaoBreve,
                AnoFundacao = AnoFundacao,
                Telefone = Telefone,
                WhatsApp = WhatsApp,
                Email = Email,
                Endereco = Endereco,
                Cidade = Cidade,
                Estado = Estado,
                CEP = CEP,
                Pais = Pais,
                Website = Website,
                Facebook = Facebook,
                Instagram = Instagram,
                Twitter = Twitter,
                LinkedIn = LinkedIn,
                HorarioCheckin = HorarioCheckin,
                HorarioCheckout = HorarioCheckout,
                Ativo = Ativo
            };
        }
    }

    public class EmpresaFotoViewModel
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "A URL da foto é obrigatória")]
        [StringLength(255, ErrorMessage = "A URL deve ter no máximo 255 caracteres")]
        [Display(Name = "URL da Foto")]
        public string FotoUrl { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(100, ErrorMessage = "O texto alternativo deve ter no máximo 100 caracteres")]
        [Display(Name = "Texto Alternativo")]
        public string? AltText { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Tipo")]
        public TipoFotoEmpresa Tipo { get; set; } = TipoFotoEmpresa.Galeria;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }

    public class EmpresaServicoViewModel
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "O nome do serviço é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome do Serviço")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(50, ErrorMessage = "O ícone deve ter no máximo 50 caracteres")]
        [Display(Name = "Ícone (classe CSS)")]
        public string? Icone { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }

    public class EmpresaPremioViewModel
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "O título do prêmio é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
        [Display(Name = "Título do Prêmio")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Range(1900, 2100, ErrorMessage = "O ano deve estar entre 1900 e 2100")]
        [Display(Name = "Ano")]
        public int? Ano { get; set; }

        [StringLength(100, ErrorMessage = "A instituição deve ter no máximo 100 caracteres")]
        [Display(Name = "Instituição")]
        public string? Instituicao { get; set; }

        [StringLength(50, ErrorMessage = "O ícone deve ter no máximo 50 caracteres")]
        [Display(Name = "Ícone (classe CSS)")]
        public string? Icone { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}
