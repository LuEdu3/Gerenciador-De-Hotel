using System.ComponentModel.DataAnnotations;

namespace GerenciadorHotel.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter entre 2 e 100 caracteres", MinimumLength = 2)]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação não coincidem")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
