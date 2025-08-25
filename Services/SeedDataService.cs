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

        public static async Task SeedAcomodacoes(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await SeedAcomodacoes(context);
        }

        public static async Task LimparEInserirAcomodacoesQuintaYpua(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await LimparEInserirAcomodacoesQuintaYpua(context);
        }

        public static async Task AtualizarImagensAcomodacoes(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await AtualizarImagensAcomodacoes(context);
        }

        private static async Task AtualizarImagensAcomodacoes(ApplicationDbContext context)
        {
            // Definir as URLs das imagens para cada acomodação
            var imagensAcomodacoes = new Dictionary<string, string>
            {
                { "Domo", "https://static.wixstatic.com/media/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg" },
                { "Charrua (Bus)", "https://static.wixstatic.com/media/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg" },
                { "Suíte com Cozinha", "https://static.wixstatic.com/media/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg" },
                { "Chalé Família", "https://static.wixstatic.com/media/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg" },
                { "Cabana", "https://static.wixstatic.com/media/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg" },
                { "Estacionamento para Overlanders", "https://static.wixstatic.com/media/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg" }
            };

            var atualizacoes = 0;
            foreach (var item in imagensAcomodacoes)
            {
                var acomodacao = context.Acomodacoes.FirstOrDefault(a => a.Nome == item.Key);
                if (acomodacao != null)
                {
                    acomodacao.ImagemPrincipalUrl = item.Value;
                    acomodacao.DataAtualizacao = DateTime.Now;
                    atualizacoes++;
                }
            }

            if (atualizacoes > 0)
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"🖼️ {atualizacoes} imagens de acomodações atualizadas e fixadas!");
            }
            else
            {
                Console.WriteLine("⚠️ Nenhuma acomodação encontrada para atualizar imagens.");
            }
        }

        private static async Task LimparEInserirAcomodacoesQuintaYpua(ApplicationDbContext context)
        {
            // Remover acomodação "Teste" se existir
            var acomodacaoTeste = context.Acomodacoes.FirstOrDefault(a => a.Nome == "Teste");
            if (acomodacaoTeste != null)
            {
                context.Acomodacoes.Remove(acomodacaoTeste);
                Console.WriteLine("🗑️ Acomodação 'Teste' removida.");
            }

            // Verificar se as acomodações da Quinta do Ypuã já existem
            var acomodacoesExistentes = new[] { "Domo", "Charrua (Bus)", "Suíte com Cozinha", "Chalé Família", "Cabana", "Estacionamento para Overlanders" };
            var existeAlguma = context.Acomodacoes.Any(a => acomodacoesExistentes.Contains(a.Nome));

            if (!existeAlguma)
            {
                var acomodacoes = new List<Acomodacao>
                {
                    new()
                    {
                        Nome = "Domo",
                        Descricao = "Uma experiência única em formato geodésico com vista panorâmica da natureza. Ideal para casais que buscam algo diferenciado e intimista.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        Preco = 280.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_442829334f1b4dd1879b3231151437a3~mv2.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Charrua (Bus)",
                        Descricao = "Acomodação única em ônibus convertido, oferecendo uma experiência alternativa e sustentável. Perfeita para aventureiros.",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 2,
                        Preco = 180.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_bus_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Suíte com Cozinha",
                        Descricao = "Suíte completa com cozinha equipada, ideal para estadias mais longas. Oferece conforto e praticidade para famílias ou casais.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        Preco = 350.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_suite_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Chalé Família",
                        Descricao = "Chalé espaçoso ideal para famílias, com múltiplas camas e área de convivência. Ambiente aconchegante em meio à natureza.",
                        QuantidadeCamasCasal = 2,
                        QuantidadeCamasSolteiro = 2,
                        Preco = 450.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_chale_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Cabana",
                        Descricao = "Cabana rústica e aconchegante, perfeita para quem busca uma conexão mais próxima com a natureza. Ideal para casais.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        Preco = 220.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_cabana_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Estacionamento para Overlanders",
                        Descricao = "Área especial para veículos de viajantes overlanders, com infraestrutura básica e acesso a banheiros compartilhados.",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 2,
                        Preco = 80.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_parking_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    }
                };

                await context.Acomodacoes.AddRangeAsync(acomodacoes);
                Console.WriteLine($"✅ {acomodacoes.Count} acomodações da Quinta do Ypuã inseridas!");
            }
            else
            {
                Console.WriteLine("⚠️ Acomodações da Quinta do Ypuã já existem no banco.");
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedAcomodacoes(ApplicationDbContext context)
        {
            // Só inserir acomodações se a tabela estiver vazia (NÃO LIMPAR DADOS EXISTENTES)
            if (!context.Acomodacoes.Any())
            {
                var acomodacoes = new List<Acomodacao>
                {
                    new()
                    {
                        Nome = "Domo",
                        Descricao = "Uma experiência única em formato geodésico com vista panorâmica da natureza. Ideal para casais que buscam algo diferenciado e intimista.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        Preco = 280.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_442829334f1b4dd1879b3231151437a3~mv2.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Charrua (Bus)",
                        Descricao = "Acomodação única em ônibus convertido, oferecendo uma experiência alternativa e sustentável. Perfeita para aventureiros.",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 2,
                        Preco = 180.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_bus_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Suíte com Cozinha",
                        Descricao = "Suíte completa com cozinha equipada, ideal para estadias mais longas. Oferece conforto e praticidade para famílias ou casais.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        Preco = 350.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_suite_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Chalé Família",
                        Descricao = "Chalé espaçoso ideal para famílias, com múltiplas camas e área de convivência. Ambiente aconchegante em meio à natureza.",
                        QuantidadeCamasCasal = 2,
                        QuantidadeCamasSolteiro = 2,
                        Preco = 450.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_chale_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Cabana",
                        Descricao = "Cabana rústica e aconchegante, perfeita para quem busca uma conexão mais próxima com a natureza. Ideal para casais.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        Preco = 220.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_cabana_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Estacionamento para Overlanders",
                        Descricao = "Área especial para veículos de viajantes overlanders, com infraestrutura básica e acesso a banheiros compartilhados.",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 2,
                        Preco = 80.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_parking_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    }
                };

                await context.Acomodacoes.AddRangeAsync(acomodacoes);
                await context.SaveChangesAsync();

                Console.WriteLine($"✅ {acomodacoes.Count} acomodações da Quinta do Ypuã inseridas com sucesso!");
            }
            else
            {
                Console.WriteLine("⚠️  Acomodações já existem no banco, pulando seed para preservar dados existentes.");
            }
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