using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Models;
using GerenciadorHotel.Data;

namespace GerenciadorHotel.Services
{
    public static class SeedDataService
    {
        public static async Task SeedEmpresaBase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await SeedEmpresaBase(context);
        }

        public static async Task ForcarAtualizacaoEmpresaBase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await ForcarAtualizacaoEmpresaBasePrivate(context);
        }

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

        public static async Task ForcarRecriacaoAcomodacoes(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await ForcarRecriacaoAcomodacoesPrivate(context);
        }

        public static async Task SeedAmenidades(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await SeedAmenidadesPrivate(context);
        }

        private static async Task SeedAmenidadesPrivate(ApplicationDbContext context)
        {
            // Só inserir amenidades se a tabela estiver vazia
            if (!context.Amenidades.Any())
            {
                var amenidades = new List<Amenidade>
                {
                    new()
                    {
                        Nome = "Wi-Fi",
                        Descricao = "Internet sem fio gratuita",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-wifi
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Ar-condicionado",
                        Descricao = "Sistema de climatização",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-thermometer-half
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "TV",
                        Descricao = "Televisão com canais por assinatura",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-tv
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Frigobar",
                        Descricao = "Refrigerador pequeno",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-snow2
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Ducha",
                        Descricao = "Banheiro com ducha",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-droplet
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Banheira",
                        Descricao = "Banheira para relaxamento",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-water
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Cozinha",
                        Descricao = "Cozinha equipada",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-house-gear
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Toalha",
                        Descricao = "Toalhas de banho incluídas",
                        ImagemUrl = "", // Usará ícone Bootstrap bi-clipboard-check
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    }
                };

                await context.Amenidades.AddRangeAsync(amenidades);
                await context.SaveChangesAsync();

                Console.WriteLine($"✅ {amenidades.Count} amenidades inseridas com sucesso!");
            }
            else
            {
                Console.WriteLine("⚠️ Amenidades já existem no banco, pulando seed para preservar dados existentes.");
            }
        }

        private static async Task ForcarAtualizacaoEmpresaBasePrivate(ApplicationDbContext context)
        {
            Console.WriteLine("🔄 Forçando atualização da empresa base...");
            
            // Busca a empresa ativa atual
            var empresaAtual = await context.Empresas.FirstOrDefaultAsync(e => e.Ativo);
            
            if (empresaAtual != null)
            {
                // Atualiza todos os campos com os dados da empresaBase
                empresaAtual.Nome = "Quinta do Ypuã";
                empresaAtual.NomeResumido = "Ypuã";
                empresaAtual.LogoUrl = "https://static.wixstatic.com/media/b87f83_9f4625b043a944daaf5fddefc7d73d0e~mv2.png/v1/fill/w_80,h_80,al_c,q_85,enc_avif,quality_auto/logo-pousada-quinta-do-ypua.png";
                empresaAtual.Slogan = "Natureza, conforto e simplicidade";
                empresaAtual.DescricaoBreve = "A pousada Quinta do Ypuã oferece ao seus clientes um recanto de aconchego e lazer, em ambiente rústico e agradável.  Ideal para quem gosta de fugir da rotina e procura um local de paz para descansar e curtir a natureza.";
                empresaAtual.DescricaoSobre = "O Ypuã tem tudo a ver com a natureza, dá para sentir a energia do lugar. Eu me preocupo se você vai comer bem, dormir bem e se vai se sentir em casa. Vou te mostrar onde encontrar os melhores frutos do mar, onde curtir a melhor praia e as melhores ondas. Mas se você não quiser fazer nada eu também conheço o melhor lugar";
                empresaAtual.AnoFundacao = 2005;
                empresaAtual.Telefone = "+55 (00) 88790-000";
                empresaAtual.WhatsApp = "+55 (00) 88790-000";
                empresaAtual.Email = "pousadaquintadoypua@gmail.com";
                empresaAtual.Endereco = "Estrada Ipua, nº 6";
                empresaAtual.Cidade = "Laguna ";
                empresaAtual.Estado = "SC";
                empresaAtual.CEP = "00000-000";
                empresaAtual.Pais = "Brasil";
                empresaAtual.Website = "https://exemplo.com";
                empresaAtual.Facebook = "https://exemplo.com";
                empresaAtual.Instagram = "https://exemplo.com";
                empresaAtual.Twitter = "https://exemplo.com";
                empresaAtual.LinkedIn = "https://exemplo.com";
                empresaAtual.HorarioCheckin = "14:00";
                empresaAtual.HorarioCheckout = "12:00";
                empresaAtual.DataAtualizacao = DateTime.Now;
                
                await context.SaveChangesAsync();
                Console.WriteLine("✅ Empresa base atualizada com sucesso!");
            }
            else
            {
                // Se não existe empresa ativa, cria uma nova
                await SeedEmpresaBase(context);
            }
        }

        private static async Task SeedEmpresaBase(ApplicationDbContext context)
        {
            // Se já existe uma empresa ativa, não faz nada (preserva dados configurados pelo usuário)
            var existeAtiva = await context.Empresas.AnyAsync(e => e.Ativo);
            if (existeAtiva)
            {
                Console.WriteLine("ℹ️ Empresa ativa já existe. Mantendo configuração atual.");
                return;
            }

            // Caso não exista nenhuma ativa, cria uma empresa base
            var existeAlguma = await context.Empresas.AnyAsync();
            if (!existeAlguma)
            {
                var empresaBase = new Empresa
                {
                    Nome = "Quinta do Ypuã",
                    NomeResumido = "Ypuã",
                    LogoUrl = "https://static.wixstatic.com/media/b87f83_9f4625b043a944daaf5fddefc7d73d0e~mv2.png/v1/fill/w_80,h_80,al_c,q_85,enc_avif,quality_auto/logo-pousada-quinta-do-ypua.png",
                    Slogan = "Natureza, conforto e simplicidade",
                    DescricaoBreve = "A pousada Quinta do Ypuã oferece ao seus clientes um recanto de aconchego e lazer, em ambiente rústico e agradável.  Ideal para quem gosta de fugir da rotina e procura um local de paz para descansar e curtir a natureza.",
                    DescricaoSobre = "O Ypuã tem tudo a ver com a natureza, dá para sentir a energia do lugar. Eu me preocupo se você vai comer bem, dormir bem e se vai se sentir em casa. Vou te mostrar onde encontrar os melhores frutos do mar, onde curtir a melhor praia e as melhores ondas. Mas se você não quiser fazer nada eu também conheço o melhor lugar",
                    AnoFundacao = 2005,
                    Telefone = "+55 (00) 88790-000",
                    WhatsApp = "+55 (00) 88790-000",
                    Email = "pousadaquintadoypua@gmail.com",
                    Endereco = "Estrada Ipua, nº 6",
                    Cidade = "Laguna ",
                    Estado = "SC",
                    CEP = "00000-000",
                    Pais = "Brasil",
                    Website = "https://exemplo.com",
                    Facebook = "https://exemplo.com",
                    Instagram = "https://exemplo.com",
                    Twitter = "https://exemplo.com",
                    LinkedIn = "https://exemplo.com",
                    HorarioCheckin = "14:00",
                    HorarioCheckout = "12:00",
                    Ativo = true,
                    DataCriacao = DateTime.Now
                };

                await context.Empresas.AddAsync(empresaBase);
                await context.SaveChangesAsync();
                Console.WriteLine("✅ Empresa base criada e ativada.");
                return;
            }

            // Existem empresas, porém nenhuma ativa: ativa a mais recente
            var empresaMaisRecente = await context.Empresas
                .OrderByDescending(e => e.DataCriacao)
                .FirstOrDefaultAsync();
            if (empresaMaisRecente != null)
            {
                empresaMaisRecente.Ativo = true;
                empresaMaisRecente.DataAtualizacao = DateTime.Now;
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ Empresa '{empresaMaisRecente.Nome}' ativada como padrão.");
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
                        QuantidadeMaximaHospedes = 3,
                        Preco = 280.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Charrua (Bus)",
                        Descricao = "Acomodação única em ônibus convertido, oferecendo uma experiência alternativa e sustentável. Perfeita para aventureiros.",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 2,
                        QuantidadeMaximaHospedes = 2,
                        Preco = 180.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Suíte com Cozinha",
                        Descricao = "Suíte completa com cozinha equipada, ideal para estadias mais longas. Oferece conforto e praticidade para famílias ou casais.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        QuantidadeMaximaHospedes = 3,
                        Preco = 350.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Chalé Família",
                        Descricao = "Chalé espaçoso ideal para famílias, com múltiplas camas e área de convivência. Ambiente aconchegante em meio à natureza.",
                        QuantidadeCamasCasal = 2,
                        QuantidadeCamasSolteiro = 2,
                        QuantidadeMaximaHospedes = 5,
                        Preco = 450.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Cabana",
                        Descricao = "Cabana rústica e aconchegante, perfeita para quem busca uma conexão mais próxima com a natureza. Ideal para casais.",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        QuantidadeMaximaHospedes = 3,
                        Preco = 220.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Estacionamento para Overlanders",
                        Descricao = "Área especial para veículos de viajantes overlanders, com infraestrutura básica e acesso a banheiros compartilhados.",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 2,
                        QuantidadeMaximaHospedes = 4,
                        Preco = 80.00m,
                        MinimoNoites = 1,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg",
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
                        QuantidadeMaximaHospedes = 3,
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
                        QuantidadeMaximaHospedes = 2,
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
                        QuantidadeMaximaHospedes = 3,
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
                        QuantidadeMaximaHospedes = 5,
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
                        QuantidadeMaximaHospedes = 3,
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
                        QuantidadeMaximaHospedes = 4,
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

        private static async Task ForcarRecriacaoAcomodacoesPrivate(ApplicationDbContext context)
        {
            Console.WriteLine("🔄 Forçando recriação de todas as acomodações...");
            
            // Remove TODAS as acomodações existentes
            var acomodacoesExistentes = context.Acomodacoes.ToList();
            if (acomodacoesExistentes.Any())
            {
                context.Acomodacoes.RemoveRange(acomodacoesExistentes);
                await context.SaveChangesAsync();
                Console.WriteLine($"🗑️ {acomodacoesExistentes.Count} acomodações existentes removidas.");
            }

            // Cria todas as acomodações da Quinta do Ypuã
            var acomodacoes = new List<Acomodacao>
            {
                new()
                {
                    Nome = "Domo",
                    Descricao = "Uma experiência única em formato geodésico com vista panorâmica da natureza. Ideal para casais que buscam algo diferenciado e intimista.",
                    QuantidadeCamasCasal = 1,
                    QuantidadeCamasSolteiro = 0,
                    QuantidadeMaximaHospedes = 3,
                    Preco = 280.00m,
                    MinimoNoites = 2,
                    Status = StatusAcomodacao.Disponivel,
                    ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg",
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new()
                {
                    Nome = "Charrua (Bus)",
                    Descricao = "Acomodação única em ônibus convertido, oferecendo uma experiência alternativa e sustentável. Perfeita para aventureiros.",
                    QuantidadeCamasCasal = 0,
                    QuantidadeCamasSolteiro = 2,
                    QuantidadeMaximaHospedes = 2,
                    Preco = 180.00m,
                    MinimoNoites = 1,
                    Status = StatusAcomodacao.Disponivel,
                    ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg",
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new()
                {
                    Nome = "Suíte com Cozinha",
                    Descricao = "Suíte completa com cozinha equipada, ideal para estadias mais longas. Oferece conforto e praticidade para famílias ou casais.",
                    QuantidadeCamasCasal = 1,
                    QuantidadeCamasSolteiro = 0,
                    QuantidadeMaximaHospedes = 3,
                    Preco = 350.00m,
                    MinimoNoites = 2,
                    Status = StatusAcomodacao.Disponivel,
                    ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg",
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new()
                {
                    Nome = "Chalé Família",
                    Descricao = "Chalé espaçoso ideal para famílias, com múltiplas camas e área de convivência. Ambiente aconchegante em meio à natureza.",
                    QuantidadeCamasCasal = 2,
                    QuantidadeCamasSolteiro = 2,
                    QuantidadeMaximaHospedes = 5,
                    Preco = 450.00m,
                    MinimoNoites = 2,
                    Status = StatusAcomodacao.Disponivel,
                    ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg",
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new()
                {
                    Nome = "Cabana",
                    Descricao = "Cabana rústica e aconchegante, perfeita para quem busca uma conexão mais próxima com a natureza. Ideal para casais.",
                    QuantidadeCamasCasal = 1,
                    QuantidadeCamasSolteiro = 0,
                    QuantidadeMaximaHospedes = 3,
                    Preco = 220.00m,
                    MinimoNoites = 1,
                    Status = StatusAcomodacao.Disponivel,
                    ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg",
                    Ativa = true,
                    DataCriacao = DateTime.Now
                },
                new()
                {
                    Nome = "Estacionamento para Overlanders",
                    Descricao = "Área especial para veículos de viajantes overlanders, com infraestrutura básica e acesso a banheiros compartilhados.",
                    QuantidadeCamasCasal = 0,
                    QuantidadeCamasSolteiro = 2,
                    QuantidadeMaximaHospedes = 4,
                    Preco = 80.00m,
                    MinimoNoites = 1,
                    Status = StatusAcomodacao.Disponivel,
                    ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg",
                    Ativa = true,
                    DataCriacao = DateTime.Now
                }
            };

            await context.Acomodacoes.AddRangeAsync(acomodacoes);
            await context.SaveChangesAsync();

            Console.WriteLine($"✅ {acomodacoes.Count} acomodações da Quinta do Ypuã criadas com sucesso!");
        }

        public static async Task AtualizarQuantidadeMaximaHospedes(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await AtualizarQuantidadeMaximaHospedesPrivate(context);
        }

        private static async Task AtualizarQuantidadeMaximaHospedesPrivate(ApplicationDbContext context)
        {
            Console.WriteLine("🔄 Atualizando quantidade máxima de hóspedes das acomodações existentes...");
            
            var acomodacoes = await context.Acomodacoes.ToListAsync();
            
            if (acomodacoes.Any())
            {
                foreach (var acomodacao in acomodacoes)
                {
                    switch (acomodacao.Nome)
                    {
                        case "Domo":
                            acomodacao.QuantidadeMaximaHospedes = 3;
                            break;
                        case "Charrua (Bus)":
                            acomodacao.QuantidadeMaximaHospedes = 2;
                            break;
                        case "Suíte com Cozinha":
                            acomodacao.QuantidadeMaximaHospedes = 3;
                            break;
                        case "Chalé Família":
                            acomodacao.QuantidadeMaximaHospedes = 5;
                            break;
                        case "Cabana":
                            acomodacao.QuantidadeMaximaHospedes = 3;
                            break;
                        case "Estacionamento para Overlanders":
                            acomodacao.QuantidadeMaximaHospedes = 4;
                            break;
                        default:
                            // Para outras acomodações, calcular baseado nas camas
                            acomodacao.QuantidadeMaximaHospedes = (acomodacao.QuantidadeCamasCasal * 2) + acomodacao.QuantidadeCamasSolteiro;
                            if (acomodacao.QuantidadeMaximaHospedes == 0)
                                acomodacao.QuantidadeMaximaHospedes = 1; // Mínimo 1 hóspede
                            break;
                    }
                    acomodacao.DataAtualizacao = DateTime.Now;
                }

                await context.SaveChangesAsync();
                Console.WriteLine($"✅ {acomodacoes.Count} acomodações atualizadas com quantidade máxima de hóspedes!");
                
                // Mostrar detalhes das atualizações
                foreach (var acomodacao in acomodacoes)
                {
                    Console.WriteLine($"   - {acomodacao.Nome}: {acomodacao.QuantidadeMaximaHospedes} hóspedes");
                }
            }
            else
            {
                Console.WriteLine("⚠️ Nenhuma acomodação encontrada para atualizar.");
            }
        }

        public static async Task CriarImagensAcomodacoes(ApplicationDbContext context)
        {
            var acomodacoesSemImagens = await context.Acomodacoes
                .Where(a => a.Ativa && !string.IsNullOrEmpty(a.ImagemPrincipalUrl) && !a.Imagens.Any())
                .ToListAsync();

            if (acomodacoesSemImagens.Any())
            {
                foreach (var acomodacao in acomodacoesSemImagens)
                {
                    var imagemAcomodacao = new ImagemAcomodacao
                    {
                        AcomodacaoId = acomodacao.Id,
                        ImagemUrl = acomodacao.ImagemPrincipalUrl!,
                        Titulo = $"Imagem Principal - {acomodacao.Nome}",
                        Descricao = $"Foto principal da acomodação {acomodacao.Nome}",
                        Ordem = 1,
                        Ativa = true,
                        DataUpload = DateTime.Now
                    };

                    context.ImagensAcomodacao.Add(imagemAcomodacao);
                }

                await context.SaveChangesAsync();
                Console.WriteLine($"🖼️ {acomodacoesSemImagens.Count} registros de imagens de acomodações criados!");
            }
            else
            {
                Console.WriteLine("ℹ️ Todas as acomodações já possuem imagens registradas.");
            }
        }
    }
}