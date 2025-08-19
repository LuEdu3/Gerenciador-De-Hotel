using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using GerenciadorHotel.Models;
using GerenciadorHotel.ViewModels;
using GerenciadorHotel.Data;

namespace GerenciadorHotel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage) });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { errors = new[] { "Email não encontrado." } });
            }

            if (!user.Ativo)
            {
                return BadRequest(new { errors = new[] { "Usuário inativo." } });
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return BadRequest(new { errors = new[] { "Senha incorreta." } });
            }

            // Atualiza último login
            user.UltimoLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            // Obtém as roles do usuário
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { 
                success = true,
                user = new { 
                    nome = user.Nome,
                    email = user.Email,
                    nivelAcesso = user.NivelAcesso,
                    roles = roles
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage) });
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return BadRequest(new { errors = new[] { "Este email já está cadastrado." } });
            }

            var user = new ApplicationUser
            {
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Email = model.Email,
                UserName = model.Email,
                NivelAcesso = NivelAcesso.Hospede,
                Ativo = true,
                DataCadastro = DateTime.Now,
                EmailConfirmed = true // Como não temos confirmação de email ainda
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }

            // Adiciona o usuário ao role de Hóspede
            await _userManager.AddToRoleAsync(user, "Hospede");

            // Faz login automático após o registro
            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { success = true });
        }
    }
}
