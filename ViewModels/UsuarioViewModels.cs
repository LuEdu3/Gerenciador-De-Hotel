using System.ComponentModel.DataAnnotations;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.ViewModels
{
    public class UsuarioListViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public NivelAcesso NivelAcesso { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
    public string Roles { get; set; } = string.Empty;
    public DateTime? UltimoLogin { get; set; }
    public string NomeCompleto => $"{Nome} {Sobrenome}";
    }

    public class UsuarioDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public NivelAcesso NivelAcesso { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    public string NomeCompleto => $"{Nome} {Sobrenome}";
    public DateTime? UltimoLogin { get; set; }
    }

    public class CriarUsuarioViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo 100 caracteres")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação não coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nível de acesso é obrigatório")]
        [Display(Name = "Nível de Acesso")]
        public NivelAcesso NivelAcesso { get; set; }
    }

    public class EditarUsuarioViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo 100 caracteres")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nível de acesso é obrigatório")]
        [Display(Name = "Nível de Acesso")]
        public NivelAcesso NivelAcesso { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}
