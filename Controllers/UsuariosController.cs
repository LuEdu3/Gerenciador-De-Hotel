using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Models;
using GerenciadorHotel.ViewModels;

namespace GerenciadorHotel.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UsuariosController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string busca, string nivelAcesso, string status)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(busca))
            {
                var buscaNormalizada = busca.Trim().ToLower();
                query = query.Where(u =>
                    u.Nome.ToLower().Contains(buscaNormalizada) ||
                    u.Sobrenome.ToLower().Contains(buscaNormalizada) ||
                    u.Email.ToLower().Contains(buscaNormalizada)
                );
            }
            if (!string.IsNullOrWhiteSpace(nivelAcesso))
            {
                if (Enum.TryParse<NivelAcesso>(nivelAcesso, out var nivel))
                {
                    query = query.Where(u => u.NivelAcesso == nivel);
                }
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status == "ativo") query = query.Where(u => u.Ativo);
                else if (status == "inativo") query = query.Where(u => !u.Ativo);
            }
            var usuarios = await query.OrderBy(u => u.Nome).ToListAsync();

            var usuariosViewModel = new List<UsuarioListViewModel>();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usuariosViewModel.Add(new UsuarioListViewModel
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Sobrenome = user.Sobrenome,
                    Email = user.Email,
                    NivelAcesso = user.NivelAcesso,
                    Ativo = user.Ativo,
                    DataCadastro = user.DataCadastro,
                    Roles = string.Join(", ", roles),
                    UltimoLogin = user.UltimoLogin
                });
            }

            return View(usuariosViewModel);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new UsuarioDetailsViewModel
            {
                Id = user.Id,
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                Email = user.Email,
                NivelAcesso = user.NivelAcesso,
                Ativo = user.Ativo,
                DataCadastro = user.DataCadastro,
                UltimaAtualizacao = user.UltimaAtualizacao,
                Roles = roles.ToList(),
                UltimoLogin = user.UltimoLogin
            };

            return View(viewModel);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CriarUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    NivelAcesso = model.NivelAcesso,
                    Ativo = true,
                    DataCadastro = DateTime.Now,
                    EmailConfirmed = true // Admin cria com email já confirmado
                };

                var result = await _userManager.CreateAsync(user, model.Senha);

                if (result.Succeeded)
                {
                    // Adicionar role baseada no nível de acesso
                    string roleName = model.NivelAcesso switch
                    {
                        NivelAcesso.Administrador => "Administrador",
                        NivelAcesso.Recepcionista => "Recepcionista",
                        NivelAcesso.Hospede => "Hospede",
                        _ => "Hospede"
                    };

                    await _userManager.AddToRoleAsync(user, roleName);

                    _logger.LogInformation($"Usuário {user.Email} criado pelo administrador");
                    TempData["Sucesso"] = "Usuário criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditarUsuarioViewModel
            {
                Id = user.Id,
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                Email = user.Email,
                NivelAcesso = user.NivelAcesso,
                Ativo = user.Ativo
            };

            return View(model);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditarUsuarioViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Atualizar dados do usuário
                user.Nome = model.Nome;
                user.Sobrenome = model.Sobrenome;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.NivelAcesso = model.NivelAcesso;
                user.Ativo = model.Ativo;
                user.UltimaAtualizacao = DateTime.Now;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Atualizar roles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);

                    string newRoleName = model.NivelAcesso switch
                    {
                        NivelAcesso.Administrador => "Administrador",
                        NivelAcesso.Recepcionista => "Recepcionista",
                        NivelAcesso.Hospede => "Hospede",
                        _ => "Hospede"
                    };

                    await _userManager.AddToRoleAsync(user, newRoleName);

                    _logger.LogInformation($"Usuário {user.Email} atualizado pelo administrador");
                    TempData["Sucesso"] = "Usuário atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new UsuarioDetailsViewModel
            {
                Id = user.Id,
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                Email = user.Email,
                NivelAcesso = user.NivelAcesso,
                Ativo = user.Ativo,
                DataCadastro = user.DataCadastro,
                UltimaAtualizacao = user.UltimaAtualizacao,
                Roles = roles.ToList()
            };

            return View(viewModel);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var currentUserId = _userManager.GetUserId(User);
            if (user != null)
            {
                if (user.Id == currentUserId)
                {
                    TempData["Erro"] = "Você não pode excluir o próprio usuário logado.";
                    return RedirectToAction(nameof(Index));
                }
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Usuário {user.Email} removido pelo administrador");
                    TempData["Sucesso"] = "Usuário removido com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Erro ao remover usuário.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetSenha(string id, string novaSenha, string confirmarSenha)
        {
            if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6 || novaSenha != confirmarSenha)
            {
                TempData["Erro"] = "A senha deve ter pelo menos 6 caracteres e coincidir com a confirmação.";
                return RedirectToAction("Details", new { id });
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction(nameof(Index));
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, novaSenha);
            if (result.Succeeded)
            {
                TempData["Sucesso"] = "Senha redefinida com sucesso!";
            }
            else
            {
                TempData["Erro"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction("Details", new { id });
        }

        // POST: Usuarios/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Ativo = !user.Ativo;
            user.UltimaAtualizacao = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var status = user.Ativo ? "ativado" : "desativado";
                TempData["Sucesso"] = $"Usuário {status} com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao alterar status do usuário.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
