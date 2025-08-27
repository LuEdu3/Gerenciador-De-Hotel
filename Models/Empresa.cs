using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorHotel.Models
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nome da Empresa")]
        public string? Nome { get; set; }

        [MaxLength(50)]
        [Display(Name = "Nome Resumido")]
        public string? NomeResumido { get; set; }

        [MaxLength(255)]
        [Display(Name = "URL da Logo")]
        public string? LogoUrl { get; set; }

        [MaxLength(500)]
        [Display(Name = "Slogan")]
        public string? Slogan { get; set; }

        [MaxLength(2000)]
        [Display(Name = "Descrição Sobre")]
        public string? DescricaoSobre { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Descrição Breve")]
        public string? DescricaoBreve { get; set; }

        [Display(Name = "Ano de Fundação")]
        public int? AnoFundacao { get; set; }

        [MaxLength(100)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        [Display(Name = "WhatsApp")]
        public string? WhatsApp { get; set; }

        [MaxLength(100)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [MaxLength(200)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [MaxLength(100)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [MaxLength(50)]
        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [MaxLength(20)]
        [Display(Name = "CEP")]
        public string? CEP { get; set; }

        [MaxLength(100)]
        [Display(Name = "País")]
        public string? Pais { get; set; }

        [MaxLength(100)]
        [Display(Name = "Website")]
        public string? Website { get; set; }

        [MaxLength(100)]
        [Display(Name = "Facebook")]
        public string? Facebook { get; set; }

        [MaxLength(100)]
        [Display(Name = "Instagram")]
        public string? Instagram { get; set; }

        [MaxLength(100)]
        [Display(Name = "Twitter")]
        public string? Twitter { get; set; }

        [MaxLength(100)]
        [Display(Name = "LinkedIn")]
        public string? LinkedIn { get; set; }

        [MaxLength(50)]
        [Display(Name = "Check-in")]
        public string? HorarioCheckin { get; set; }

        [MaxLength(50)]
        [Display(Name = "Check-out")]
        public string? HorarioCheckout { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        // Propriedades de navegação
        public ICollection<EmpresaFoto>? Fotos { get; set; }
        public ICollection<EmpresaServico>? Servicos { get; set; }
        public ICollection<EmpresaPremio>? Premios { get; set; }
    }

    public class EmpresaFoto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa? Empresa { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "URL da Foto")]
        public string? FotoUrl { get; set; }

        [MaxLength(255)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [MaxLength(100)]
        [Display(Name = "Alt Text")]
        public string? AltText { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Tipo")]
        public TipoFotoEmpresa Tipo { get; set; } = TipoFotoEmpresa.Galeria;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public class EmpresaServico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa? Empresa { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Nome do Serviço")]
        public string? Nome { get; set; }

        [MaxLength(500)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [MaxLength(50)]
        [Display(Name = "Ícone")]
        public string? Icone { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public class EmpresaPremio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa? Empresa { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Título do Prêmio")]
        public string? Titulo { get; set; }

        [MaxLength(500)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Ano")]
        public int? Ano { get; set; }

        [MaxLength(100)]
        [Display(Name = "Instituição")]
        public string? Instituicao { get; set; }

        [MaxLength(50)]
        [Display(Name = "Ícone")]
        public string? Icone { get; set; }

        [Display(Name = "Ordem")]
        public int Ordem { get; set; } = 0;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public enum TipoFotoEmpresa
    {
        [Display(Name = "Logo")]
        Logo = 1,

        [Display(Name = "Capa/Hero")]
        Hero = 2,

        [Display(Name = "Sobre")]
        Sobre = 3,

        [Display(Name = "Galeria")]
        Galeria = 4,

        [Display(Name = "Fachada")]
        Fachada = 5,

        [Display(Name = "Interior")]
        Interior = 6,

        [Display(Name = "Outras")]
        Outras = 7
    }
}
