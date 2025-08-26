using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Models;
using GerenciadorHotel.Data;
using System.Text.Json;

namespace GerenciadorHotel.Services
{
    public static class SeedDataService
    {
        // DTO simples para backup/restauração
        private class AcomodacaoBackupDto
        {
            public string Nome { get; set; } = string.Empty;
            public string? Descricao { get; set; }
            public int QuantidadeCamasCasal { get; set; }
            public int QuantidadeCamasSolteiro { get; set; }
            public int QuantidadeMaximaHospedes { get; set; }
            public decimal Preco { get; set; }
            public int MinimoNoites { get; set; }
            public string? ImagemPrincipalUrl { get; set; }
            public bool Ativa { get; set; } = true;
            public string HoraCheckIn { get; set; } = "14:00";
            public string HoraCheckOut { get; set; } = "10:00";
            public List<string> Amenidades { get; set; } = new();
        }

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

        public static async Task ImportarAcomodacoesDeBackup(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await ImportarAcomodacoesDeBackupPrivate(context);
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

        public static async Task SeedAcomodacaoAmenidades(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await SeedAcomodacaoAmenidadesPrivate(context);
        }

        public static async Task GarantirImagemPrincipalAcomodacoes(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GerenciadorHotel.Data.ApplicationDbContext>();
            await GarantirImagemPrincipalAcomodacoesPrivate(context);
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

        private static async Task SeedAcomodacaoAmenidadesPrivate(ApplicationDbContext context)
        {
            // Não remove nada. Apenas garante associações básicas para acomodações conhecidas.
            if (!context.Acomodacoes.Any() || !context.Amenidades.Any())
            {
                return; // Sem dados para associar
            }

            // Mapa de amenidades por nome da acomodação
            var mapa = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["Domo"] = new[] { "Wi-Fi", "Ar-condicionado", "TV", "Ducha" },
                ["Charrua (Bus)"] = new[] { "Wi-Fi", "Ducha" },
                ["Suíte com Cozinha"] = new[] { "Wi-Fi", "Ar-condicionado", "TV", "Cozinha", "Ducha", "Toalha" },
                ["Chalé Família"] = new[] { "Wi-Fi", "Ar-condicionado", "TV", "Ducha", "Toalha" },
                ["Cabana"] = new[] { "Wi-Fi", "Ducha", "Toalha" },
                ["Estacionamento para Overlanders"] = new[] { "Ducha" }
            };

            var amenidades = await context.Amenidades.Where(a => a.Ativa).ToListAsync();
            var acomodacoes = await context.Acomodacoes.ToListAsync();

            int novos = 0;
            foreach (var a in acomodacoes)
            {
                if (!mapa.TryGetValue(a.Nome, out var nomesAmens))
                    continue; // só cuidamos das padrão

                var idsDesejados = amenidades
                    .Where(am => nomesAmens.Contains(am.Nome, StringComparer.OrdinalIgnoreCase))
                    .Select(am => am.Id)
                    .ToHashSet();

                if (!idsDesejados.Any()) continue;

                var existentes = await context.AcomodacaoAmenidades
                    .Where(aa => aa.AcomodacaoId == a.Id)
                    .Select(aa => aa.AmenidadeId)
                    .ToListAsync();

                foreach (var amenId in idsDesejados)
                {
                    if (!existentes.Contains(amenId))
                    {
                        await context.AcomodacaoAmenidades.AddAsync(new AcomodacaoAmenidade
                        {
                            AcomodacaoId = a.Id,
                            AmenidadeId = amenId,
                            DataAssociacao = DateTime.Now
                        });
                        novos++;
                    }
                }
            }

            if (novos > 0)
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"🔗 {novos} associações Acomodacao-Amenidade criadas.");
            }
            else
            {
                Console.WriteLine("ℹ️ Associações Acomodacao-Amenidade já estão configuradas. Nenhuma criada.");
            }
        }

        private static async Task ImportarAcomodacoesDeBackupPrivate(ApplicationDbContext context)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Script", "backup_acomodacoes.json");
                if (!File.Exists(path))
                {
                    Console.WriteLine("ℹ️ Nenhum backup de acomodações encontrado em Script/backup_acomodacoes.json.");
                    return;
                }

                var json = await File.ReadAllTextAsync(path);
                var dados = JsonSerializer.Deserialize<List<AcomodacaoBackupDto>>(json) ?? new List<AcomodacaoBackupDto>();
                if (!dados.Any())
                {
                    Console.WriteLine("ℹ️ Arquivo de backup vazio. Nada a importar.");
                    return;
                }

                var amenidades = await context.Amenidades.ToListAsync();
                int criadas = 0, atualizadas = 0, assocNovas = 0;

                foreach (var dto in dados)
                {
                    var existente = await context.Acomodacoes.FirstOrDefaultAsync(a => a.Nome == dto.Nome);
                    TimeSpan TryParseHora(string s, TimeSpan def)
                        => TimeSpan.TryParse(s, out var ts) ? ts : def;

                    if (existente == null)
                    {
                        var nova = new Acomodacao
                        {
                            Nome = dto.Nome,
                            Descricao = dto.Descricao,
                            QuantidadeCamasCasal = dto.QuantidadeCamasCasal,
                            QuantidadeCamasSolteiro = dto.QuantidadeCamasSolteiro,
                            QuantidadeMaximaHospedes = dto.QuantidadeMaximaHospedes > 0 ? dto.QuantidadeMaximaHospedes : 1,
                            Preco = dto.Preco,
                            MinimoNoites = dto.MinimoNoites > 0 ? dto.MinimoNoites : 1,
                            ImagemPrincipalUrl = string.IsNullOrWhiteSpace(dto.ImagemPrincipalUrl) ? null : dto.ImagemPrincipalUrl,
                            Ativa = dto.Ativa,
                            HoraCheckIn = TryParseHora(dto.HoraCheckIn ?? "14:00", new TimeSpan(14,0,0)),
                            HoraCheckOut = TryParseHora(dto.HoraCheckOut ?? "10:00", new TimeSpan(10,0,0)),
                            Status = StatusAcomodacao.Disponivel,
                            DataCriacao = DateTime.Now
                        };
                        // Fallback de imagem para nomes conhecidos
                        if (string.IsNullOrEmpty(nova.ImagemPrincipalUrl))
                        {
                            nova.ImagemPrincipalUrl = ObterImagemPadraoPorNome(nova.Nome);
                        }
                        await context.Acomodacoes.AddAsync(nova);
                        await context.SaveChangesAsync();
                        existente = nova;
                        criadas++;
                    }
                    else
                    {
                        // Atualiza campos principais (sem mexer em imagens existentes aqui)
                        existente.Descricao = dto.Descricao ?? existente.Descricao;
                        existente.QuantidadeCamasCasal = dto.QuantidadeCamasCasal;
                        existente.QuantidadeCamasSolteiro = dto.QuantidadeCamasSolteiro;
                        existente.QuantidadeMaximaHospedes = dto.QuantidadeMaximaHospedes > 0 ? dto.QuantidadeMaximaHospedes : existente.QuantidadeMaximaHospedes;
                        existente.Preco = dto.Preco;
                        existente.MinimoNoites = dto.MinimoNoites > 0 ? dto.MinimoNoites : existente.MinimoNoites;
                        existente.HoraCheckIn = TryParseHora(dto.HoraCheckIn ?? "14:00", existente.HoraCheckIn);
                        existente.HoraCheckOut = TryParseHora(dto.HoraCheckOut ?? "10:00", existente.HoraCheckOut);
                        // Se backup não traz imagem e a existente está vazia, aplicar fallback
                        if (string.IsNullOrWhiteSpace(dto.ImagemPrincipalUrl) && string.IsNullOrWhiteSpace(existente.ImagemPrincipalUrl))
                        {
                            existente.ImagemPrincipalUrl = ObterImagemPadraoPorNome(existente.Nome) ?? existente.ImagemPrincipalUrl;
                        }
                        existente.DataAtualizacao = DateTime.Now;
                        atualizadas++;
                    }

                    // Associa amenidades por nome
                    var nomesAmens = dto.Amenidades?.Distinct(StringComparer.OrdinalIgnoreCase).ToArray() ?? Array.Empty<string>();
                    if (nomesAmens.Length > 0)
                    {
                        var idsDesejados = amenidades
                            .Where(am => nomesAmens.Contains(am.Nome, StringComparer.OrdinalIgnoreCase))
                            .Select(am => am.Id)
                            .ToHashSet();
                        var existentes = await context.AcomodacaoAmenidades
                            .Where(aa => aa.AcomodacaoId == existente.Id)
                            .Select(aa => aa.AmenidadeId)
                            .ToListAsync();
                        foreach (var idAm in idsDesejados)
                        {
                            if (!existentes.Contains(idAm))
                            {
                                await context.AcomodacaoAmenidades.AddAsync(new AcomodacaoAmenidade
                                {
                                    AcomodacaoId = existente.Id,
                                    AmenidadeId = idAm,
                                    DataAssociacao = DateTime.Now
                                });
                                assocNovas++;
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine($"📥 Importação de acomodações: {criadas} criadas, {atualizadas} atualizadas, {assocNovas} associações novas.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Falha ao importar backup de acomodações: {ex.Message}");
            }
        }

        private static async Task GarantirImagemPrincipalAcomodacoesPrivate(ApplicationDbContext context)
        {
            var semImagem = await context.Acomodacoes
                .Where(a => a.Ativa && (a.ImagemPrincipalUrl == null || a.ImagemPrincipalUrl == ""))
                .ToListAsync();

            int atualizadas = 0;
            foreach (var a in semImagem)
            {
                var fallback = ObterImagemPadraoPorNome(a.Nome);
                if (!string.IsNullOrEmpty(fallback))
                {
                    a.ImagemPrincipalUrl = fallback;
                    a.DataAtualizacao = DateTime.Now;
                    atualizadas++;
                }
            }

            if (atualizadas > 0)
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"🖼️ Imagem principal preenchida para {atualizadas} acomodação(ões) sem foto.");
            }
        }

        private static string? ObterImagemPadraoPorNome(string nome)
        {
            return nome switch
            {
                "Domo" => "https://static.wixstatic.com/media/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg",
                "Charrua (Bus)" => "https://static.wixstatic.com/media/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_5580c08771c841089ccc440a82c2f298~mv2.jpeg",
                "Suíte com Cozinha" => "https://static.wixstatic.com/media/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_bfc66e6435f34c23bfd60e2fccb3d499~mv2.jpg",
                "Chalé Família" => "https://static.wixstatic.com/media/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_d943676e56f24781b4aad20256b75eef~mv2.jpg",
                "Cabana" => "https://static.wixstatic.com/media/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_23a56936773e4f7f812d0543c078138c~mv2.jpg",
                "Estacionamento para Overlanders" => "https://static.wixstatic.com/media/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_f4b318355c704575a4a6917c1a2f7401~mv2.jpg",
                _ => null
            };
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
                empresaAtual.HorarioCheckout = "10:00";
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
                    HorarioCheckout = "10:00",
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

            // Definições padrão da Quinta do Ypuã
            var padrao = new List<Acomodacao>
            {
                new()
                {
                    Nome = "Domo",
                    Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                    QuantidadeCamasCasal = 1,
                    QuantidadeCamasSolteiro = 0,
                    QuantidadeMaximaHospedes = 3,
                    Preco = 590.00m,
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

            int inseridas = 0;
            foreach (var def in padrao)
            {
                var existe = context.Acomodacoes.Any(a => a.Nome == def.Nome);
                if (!existe)
                {
                    await context.Acomodacoes.AddAsync(def);
                    inseridas++;
                }
            }

            if (inseridas > 0)
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"✅ {inseridas} acomodações padrão inseridas (faltantes).");
            }
            else
            {
                Console.WriteLine("ℹ️ Todas as acomodações padrão já existem.");
            }
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
                        Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 0,
                        QuantidadeMaximaHospedes = 3,
                        Preco = 590.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg/v1/fill/w_649,h_408,q_85,usm_0.66_1.00_0.01/b87f83_0db328063a8c4b4ea1bb3dff437e8e46~mv2.jpeg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Charrua (Bus)",
                        Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 1,
                        QuantidadeMaximaHospedes = 2,
                        Preco = 490.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_bus_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Suíte com Cozinha",
                        Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 1,
                        QuantidadeMaximaHospedes = 3,
                        Preco = 390.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_suite_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Chalé Família",
                        Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                        QuantidadeCamasCasal = 2,
                        QuantidadeCamasSolteiro = 1,
                        QuantidadeMaximaHospedes = 5,
                        Preco = 590.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_chale_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Cabana",
                        Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                        QuantidadeCamasCasal = 1,
                        QuantidadeCamasSolteiro = 1,
                        QuantidadeMaximaHospedes = 3,
                        Preco = 490.00m,
                        MinimoNoites = 2,
                        Status = StatusAcomodacao.Disponivel,
                        ImagemPrincipalUrl = "https://static.wixstatic.com/media/b87f83_cabana_example.jpg",
                        Ativa = true,
                        DataCriacao = DateTime.Now
                    },
                    new()
                    {
                        Nome = "Estacionamento para Overlanders",
                        Descricao = "Obs: Os valores exibidos no site estão sujeitos a constantes atualizações. Nos feriados e datas comemorativas o valor da diária também é diferenciado. Para mais detalhes entre em contato por telefone. O Domo é a grande novidade da pousada. Uma acomodação totalmente diferenciada construída nos padrões arquitetônicos dos domos geodésicos modernos. (Arraste a imagem de capa para o lado para ver mais fotos da acomodação)",
                        QuantidadeCamasCasal = 0,
                        QuantidadeCamasSolteiro = 0,
                        QuantidadeMaximaHospedes = 4,
                        Preco = 100.00m,
                        MinimoNoites = 2,
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