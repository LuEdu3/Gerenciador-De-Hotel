using Microsoft.AspNetCore.Identity;
using GerenciadorHotel.Models;
using GerenciadorHotel.Data;

namespace GerenciadorHotel.Services
{
    public static class SeedDataService
    {
        public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Criar roles se não existirem
            string[] roleNames = { "Administrador", "Recepcionista", "Hospede" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Criar usuário administrador padrão
            var adminEmail = "admin@hotel.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nome = "Administrador",
                    Sobrenome = "Sistema",
                    NivelAcesso = NivelAcesso.Administrador,
                    Ativo = true,
                    DataCadastro = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrador");
                }
            }

            // Criar usuário recepcionista padrão
            var recepcionistaEmail = "recepcionista@hotel.com";
            var recepcionistaUser = await userManager.FindByEmailAsync(recepcionistaEmail);

            if (recepcionistaUser == null)
            {
                var recepcionista = new ApplicationUser
                {
                    UserName = recepcionistaEmail,
                    Email = recepcionistaEmail,
                    Nome = "Recepcionista",
                    Sobrenome = "Teste",
                    NivelAcesso = NivelAcesso.Recepcionista,
                    Ativo = true,
                    DataCadastro = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(recepcionista, "Recep123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(recepcionista, "Recepcionista");
                }
            }
        }

        public static async Task SeedPaises(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await SeedPaises(context);
        }

        private static async Task SeedPaises(ApplicationDbContext context)
        {
            // Só inserir países se a tabela estiver vazia (NÃO LIMPAR DADOS EXISTENTES)
            if (!context.Paises.Any())
            {
                var paises = new List<Pais>
                {
                    new() { Nome = "Brasil", Codigo = "br" },
                    new() { Nome = "Estados Unidos", Codigo = "us" },
                    new() { Nome = "Argentina", Codigo = "ar" },
                    new() { Nome = "Chile", Codigo = "cl" },
                    new() { Nome = "França", Codigo = "fr" },
                    new() { Nome = "Alemanha", Codigo = "de" },
                    new() { Nome = "Reino Unido", Codigo = "gb" },
                    new() { Nome = "Espanha", Codigo = "es" },
                    new() { Nome = "Itália", Codigo = "it" },
                    new() { Nome = "Portugal", Codigo = "pt" }
                };

                await context.Paises.AddRangeAsync(paises);
                await context.SaveChangesAsync();

                Console.WriteLine($"✅ {paises.Count} países inseridos com sucesso!");
            }
            else
            {
                Console.WriteLine("⚠️  Países já existem no banco, pulando seed para preservar dados existentes.");
            }
        }
    }
}