using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorHotel.Models
{
    public enum NivelAcesso
    {
        [Display(Name = "Hóspede")]
        Hospede = 1,
        [Display(Name = "Recepcionista")]
        Recepcionista = 2,
        [Display(Name = "Administrador")]
        Administrador = 3
    }

    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo 100 caracteres")]
        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; } = string.Empty;

        [Display(Name = "Nível de Acesso")]
        public NivelAcesso NivelAcesso { get; set; } = NivelAcesso.Hospede;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Display(Name = "Última Atualização")]
        public DateTime? UltimaAtualizacao { get; set; }

    // Propriedade calculada para nome completo
    [Display(Name = "Nome Completo")]
    public string NomeCompleto => $"{Nome} {Sobrenome}";

    [Display(Name = "Último Login")]
    public DateTime? UltimoLogin { get; set; }
    }
}
