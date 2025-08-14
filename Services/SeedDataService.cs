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
            await SeedPaises(context);a
        }

        private static async Task SeedPaises(ApplicationDbContext context)
        {
            // Limpar países existentes se houver
            if (context.Paises.Any())
            {
                context.Paises.RemoveRange(context.Paises);
                await context.SaveChangesAsync();
            }

            var paises = new List<Pais>
            {
                new() { Nome = "Abecásia, Geórgia", Codigo = "xa" },
                new() { Nome = "Afeganistão", Codigo = "af" },
                new() { Nome = "Albânia", Codigo = "al" },
                new() { Nome = "Argélia", Codigo = "dz" },
                new() { Nome = "Samoa Americana", Codigo = "as" },
                new() { Nome = "Andorra", Codigo = "ad" },
                new() { Nome = "Angola", Codigo = "ao" },
                new() { Nome = "Anguilla", Codigo = "ai" },
                new() { Nome = "Antártica", Codigo = "aq" },
                new() { Nome = "Antígua e Barbuda", Codigo = "ag" },
                new() { Nome = "Argentina", Codigo = "ar" },
                new() { Nome = "Armênia", Codigo = "am" },
                new() { Nome = "Aruba", Codigo = "aw" },
                new() { Nome = "Austrália", Codigo = "au" },
                new() { Nome = "Áustria", Codigo = "at" },
                new() { Nome = "Azerbaijão", Codigo = "az" },
                new() { Nome = "Bahamas", Codigo = "bs" },
                new() { Nome = "Bahrein", Codigo = "bh" },
                new() { Nome = "Bangladesh", Codigo = "bd" },
                new() { Nome = "Barbados", Codigo = "bb" },
                new() { Nome = "Bielorrússia", Codigo = "by" },
                new() { Nome = "Bélgica", Codigo = "be" },
                new() { Nome = "Belize", Codigo = "bz" },
                new() { Nome = "Benim", Codigo = "bj" },
                new() { Nome = "Bermudas", Codigo = "bm" },
                new() { Nome = "Butão", Codigo = "bt" },
                new() { Nome = "Bolívia", Codigo = "bo" },
                new() { Nome = "Bonaire S. Eustáquio e Saba", Codigo = "bq" },
                new() { Nome = "Bósnia e Herzegovina", Codigo = "ba" },
                new() { Nome = "Botsuana", Codigo = "bw" },
                new() { Nome = "Ilha Bouvet", Codigo = "bv" },
                new() { Nome = "Brasil", Codigo = "br" },
                new() { Nome = "Território Britânico do Oceano Índico", Codigo = "io" },
                new() { Nome = "Brunei Darussalã", Codigo = "bn" },
                new() { Nome = "Bulgária", Codigo = "bg" },
                new() { Nome = "Burquina Faso", Codigo = "bf" },
                new() { Nome = "Burundi", Codigo = "bi" },
                new() { Nome = "Camboja", Codigo = "kh" },
                new() { Nome = "Camarões", Codigo = "cm" },
                new() { Nome = "Canadá", Codigo = "ca" },
                new() { Nome = "Cabo Verde", Codigo = "cv" },
                new() { Nome = "Ilhas Caimã", Codigo = "ky" },
                new() { Nome = "República Centro-Africana", Codigo = "cf" },
                new() { Nome = "Chade", Codigo = "td" },
                new() { Nome = "Chile", Codigo = "cl" },
                new() { Nome = "China", Codigo = "cn" },
                new() { Nome = "Ilha Christmas", Codigo = "cx" },
                new() { Nome = "Ilhas Cocos (Keeling)", Codigo = "cc" },
                new() { Nome = "Colômbia", Codigo = "co" },
                new() { Nome = "Comores", Codigo = "km" },
                new() { Nome = "Congo", Codigo = "cg" },
                new() { Nome = "Ilhas Cook", Codigo = "ck" },
                new() { Nome = "Costa Rica", Codigo = "cr" },
                new() { Nome = "Costa do Marfim", Codigo = "ci" },
                new() { Nome = "Croácia", Codigo = "hr" },
                new() { Nome = "Curaçao", Codigo = "cw" },
                new() { Nome = "Chipre", Codigo = "cy" },
                new() { Nome = "República Tcheca", Codigo = "cz" },
                new() { Nome = "República Democrática do Congo", Codigo = "cd" },
                new() { Nome = "Dinamarca", Codigo = "dk" },
                new() { Nome = "Djibuti", Codigo = "dj" },
                new() { Nome = "Dominica", Codigo = "dm" },
                new() { Nome = "República Dominicana", Codigo = "do" },
                new() { Nome = "Timor-Leste", Codigo = "tl" },
                new() { Nome = "Equador", Codigo = "ec" },
                new() { Nome = "Egito", Codigo = "eg" },
                new() { Nome = "El Salvador", Codigo = "sv" },
                new() { Nome = "Guiné Equatorial", Codigo = "gq" },
                new() { Nome = "Eritreia", Codigo = "er" },
                new() { Nome = "Estônia", Codigo = "ee" },
                new() { Nome = "Etiópia", Codigo = "et" },
                new() { Nome = "Ilhas Malvinas", Codigo = "fk" },
                new() { Nome = "Ilhas Feroe", Codigo = "fo" },
                new() { Nome = "Fiji", Codigo = "fj" },
                new() { Nome = "Finlândia", Codigo = "fi" },
                new() { Nome = "França", Codigo = "fr" },
                new() { Nome = "Guiana Francesa", Codigo = "gf" },
                new() { Nome = "Polinésia Francesa", Codigo = "pf" },
                new() { Nome = "Territórios Franceses do Sul", Codigo = "tf" },
                new() { Nome = "Gabão", Codigo = "ga" },
                new() { Nome = "Gâmbia", Codigo = "gm" },
                new() { Nome = "Geórgia", Codigo = "ge" },
                new() { Nome = "Alemanha", Codigo = "de" },
                new() { Nome = "Gana", Codigo = "gh" },
                new() { Nome = "Gibraltar", Codigo = "gi" },
                new() { Nome = "Grécia", Codigo = "gr" },
                new() { Nome = "Groelândia", Codigo = "gl" },
                new() { Nome = "Granada", Codigo = "gd" },
                new() { Nome = "Guadalupe", Codigo = "gp" },
                new() { Nome = "Guam", Codigo = "gu" },
                new() { Nome = "Guatemala", Codigo = "gt" },
                new() { Nome = "Guérnesei", Codigo = "gg" },
                new() { Nome = "Guiné", Codigo = "gn" },
                new() { Nome = "Guiné-Bissau", Codigo = "gw" },
                new() { Nome = "Guiana", Codigo = "gy" },
                new() { Nome = "Haiti", Codigo = "ht" },
                new() { Nome = "Território das Ilhas Heard e McDonald", Codigo = "hm" },
                new() { Nome = "Honduras", Codigo = "hn" },
                new() { Nome = "Hong Kong", Codigo = "hk" },
                new() { Nome = "Hungria", Codigo = "hu" },
                new() { Nome = "Islândia", Codigo = "is" },
                new() { Nome = "Índia", Codigo = "in" },
                new() { Nome = "Indonésia", Codigo = "id" },
                new() { Nome = "Irã", Codigo = "ir" },
                new() { Nome = "Iraque", Codigo = "iq" },
                new() { Nome = "Irlanda", Codigo = "ie" },
                new() { Nome = "Ilha de Man", Codigo = "im" },
                new() { Nome = "Israel", Codigo = "il" },
                new() { Nome = "Itália", Codigo = "it" },
                new() { Nome = "Jamaica", Codigo = "jm" },
                new() { Nome = "Japão", Codigo = "jp" },
                new() { Nome = "Jersey", Codigo = "je" },
                new() { Nome = "Jordânia", Codigo = "jo" },
                new() { Nome = "Cazaquistão", Codigo = "kz" },
                new() { Nome = "Quênia", Codigo = "ke" },
                new() { Nome = "Kiribati", Codigo = "ki" },
                new() { Nome = "Kosovo", Codigo = "xk" },
                new() { Nome = "Kuwait", Codigo = "kw" },
                new() { Nome = "Quirguistão", Codigo = "kg" },
                new() { Nome = "Laos", Codigo = "la" },
                new() { Nome = "Letônia", Codigo = "lv" },
                new() { Nome = "Líbano", Codigo = "lb" },
                new() { Nome = "Lesoto", Codigo = "ls" },
                new() { Nome = "Libéria", Codigo = "lr" },
                new() { Nome = "Líbia", Codigo = "ly" },
                new() { Nome = "Liechtenstein", Codigo = "li" },
                new() { Nome = "Lituânia", Codigo = "lt" },
                new() { Nome = "Luxemburgo", Codigo = "lu" },
                new() { Nome = "Macau", Codigo = "mo" },
                new() { Nome = "Macedônia", Codigo = "mk" },
                new() { Nome = "Madagascar", Codigo = "mg" },
                new() { Nome = "Malawi", Codigo = "mw" },
                new() { Nome = "Malásia", Codigo = "my" },
                new() { Nome = "Maldivas", Codigo = "mv" },
                new() { Nome = "Mali", Codigo = "ml" },
                new() { Nome = "Malta", Codigo = "mt" },
                new() { Nome = "Ilhas Marshall", Codigo = "mh" },
                new() { Nome = "Martinica", Codigo = "mq" },
                new() { Nome = "Mauritânia", Codigo = "mr" },
                new() { Nome = "Maurício", Codigo = "mu" },
                new() { Nome = "Mayotte", Codigo = "yt" },
                new() { Nome = "México", Codigo = "mx" },
                new() { Nome = "Micronésia", Codigo = "fm" },
                new() { Nome = "Moldávia", Codigo = "md" },
                new() { Nome = "Principado de Mônaco", Codigo = "mc" },
                new() { Nome = "Mongólia", Codigo = "mn" },
                new() { Nome = "Montenegro", Codigo = "me" },
                new() { Nome = "Montserrat", Codigo = "ms" },
                new() { Nome = "Marrocos", Codigo = "ma" },
                new() { Nome = "Moçambique", Codigo = "mz" },
                new() { Nome = "Myanmar", Codigo = "mm" },
                new() { Nome = "Namíbia", Codigo = "na" },
                new() { Nome = "Nauru", Codigo = "nr" },
                new() { Nome = "Nepal", Codigo = "np" },
                new() { Nome = "Países Baixos", Codigo = "nl" },
                new() { Nome = "Nova Caledônia", Codigo = "nc" },
                new() { Nome = "Nova Zelândia", Codigo = "nz" },
                new() { Nome = "Nicarágua", Codigo = "ni" },
                new() { Nome = "Níger", Codigo = "ne" },
                new() { Nome = "Nigéria", Codigo = "ng" },
                new() { Nome = "Niue", Codigo = "nu" },
                new() { Nome = "Ilha Norfolk", Codigo = "nf" },
                new() { Nome = "Coreia do Norte", Codigo = "kp" },
                new() { Nome = "Ilhas Marianas do Norte", Codigo = "mp" },
                new() { Nome = "Noruega", Codigo = "no" },
                new() { Nome = "Omã", Codigo = "om" },
                new() { Nome = "Paquistão", Codigo = "pk" },
                new() { Nome = "Palau", Codigo = "pw" },
                new() { Nome = "Territórios Palestinos", Codigo = "ps" },
                new() { Nome = "Panamá", Codigo = "pa" },
                new() { Nome = "Papua-Nova Guiné", Codigo = "pg" },
                new() { Nome = "Paraguai", Codigo = "py" },
                new() { Nome = "Peru", Codigo = "pe" },
                new() { Nome = "Filipinas", Codigo = "ph" },
                new() { Nome = "Ilhas Pitcairn", Codigo = "pn" },
                new() { Nome = "Polônia", Codigo = "pl" },
                new() { Nome = "Portugal", Codigo = "pt" },
                new() { Nome = "Porto Rico", Codigo = "pr" },
                new() { Nome = "Qatar", Codigo = "qa" },
                new() { Nome = "Reunião", Codigo = "re" },
                new() { Nome = "Romênia", Codigo = "ro" },
                new() { Nome = "Rússia", Codigo = "ru" },
                new() { Nome = "Ruanda", Codigo = "rw" },
                new() { Nome = "São Bartolomeu", Codigo = "bl" },
                new() { Nome = "São Cristóvão e Nevis", Codigo = "kn" },
                new() { Nome = "Santa Lúcia", Codigo = "lc" },
                new() { Nome = "São Martinho", Codigo = "mf" },
                new() { Nome = "São Vicente e Granadinas", Codigo = "vc" },
                new() { Nome = "Samoa", Codigo = "ws" },
                new() { Nome = "San Marino", Codigo = "sm" },
                new() { Nome = "Arábia Saudita", Codigo = "sa" },
                new() { Nome = "Senegal", Codigo = "sn" },
                new() { Nome = "Sérvia", Codigo = "rs" },
                new() { Nome = "Seychelles", Codigo = "sc" },
                new() { Nome = "Serra Leoa", Codigo = "sl" },
                new() { Nome = "Singapura", Codigo = "sg" },
                new() { Nome = "São Martinho", Codigo = "sx" },
                new() { Nome = "Eslováquia", Codigo = "sk" },
                new() { Nome = "Eslovênia", Codigo = "si" },
                new() { Nome = "Ilhas Salomão", Codigo = "sb" },
                new() { Nome = "Somália", Codigo = "so" },
                new() { Nome = "África do Sul", Codigo = "za" },
                new() { Nome = "Ilhas Geórgia do Sul e Sandwich do Sul", Codigo = "gs" },
                new() { Nome = "Coreia do Sul", Codigo = "kr" },
                new() { Nome = "Espanha", Codigo = "es" },
                new() { Nome = "Sri Lanka", Codigo = "lk" },
                new() { Nome = "Santa Helena", Codigo = "sh" },
                new() { Nome = "Saint-Pierre e Miquelon", Codigo = "pm" },
                new() { Nome = "Sudão", Codigo = "sd" },
                new() { Nome = "Suriname", Codigo = "sr" },
                new() { Nome = "Svalbard e Jan Mayen", Codigo = "sj" },
                new() { Nome = "Suazilândia", Codigo = "sz" },
                new() { Nome = "Suécia", Codigo = "se" },
                new() { Nome = "Suíça", Codigo = "ch" },
                new() { Nome = "Síria", Codigo = "sy" },
                new() { Nome = "São Tomé e Príncipe", Codigo = "st" },
                new() { Nome = "Taiwan", Codigo = "tw" },
                new() { Nome = "Tajiquistão", Codigo = "tj" },
                new() { Nome = "Tanzânia", Codigo = "tz" },
                new() { Nome = "Tailândia", Codigo = "th" },
                new() { Nome = "Togo", Codigo = "tg" },
                new() { Nome = "Tokelau", Codigo = "tk" },
                new() { Nome = "Tonga", Codigo = "to" },
                new() { Nome = "Trinidad e Tobago", Codigo = "tt" },
                new() { Nome = "Tunísia", Codigo = "tn" },
                new() { Nome = "Turquia", Codigo = "tr" },
                new() { Nome = "Turquemenistão", Codigo = "tm" },
                new() { Nome = "Ilhas Turcas e Caicos", Codigo = "tc" },
                new() { Nome = "Tuvalu", Codigo = "tv" },
                new() { Nome = "Ilhas Virgens Britânicas", Codigo = "vg" },
                new() { Nome = "Ilhas Virgens Americanas", Codigo = "vi" },
                new() { Nome = "EUA", Codigo = "us" },
                new() { Nome = "Uganda", Codigo = "ug" },
                new() { Nome = "Ucrânia", Codigo = "ua" },
                new() { Nome = "Emirados Árabes Unidos", Codigo = "ae" },
                new() { Nome = "Reino Unido", Codigo = "gb" },
                new() { Nome = "Ilhas Menores Distantes dos Estados Unidos", Codigo = "um" },
                new() { Nome = "Uruguai", Codigo = "uy" },
                new() { Nome = "Uzbequistão", Codigo = "uz" },
                new() { Nome = "Vanuatu", Codigo = "vu" },
                new() { Nome = "Cidade do Vaticano", Codigo = "va" },
                new() { Nome = "Venezuela", Codigo = "ve" },
                new() { Nome = "Vietnã", Codigo = "vn" },
                new() { Nome = "Wallis e Futuna", Codigo = "wf" },
                new() { Nome = "Iêmen", Codigo = "ye" },
                new() { Nome = "Zâmbia", Codigo = "zm" },
                new() { Nome = "Zimbabwe", Codigo = "zw" }
            };

            await context.Paises.AddRangeAsync(paises);
            await context.SaveChangesAsync();

            Console.WriteLine($"✅ {paises.Count} países inseridos com sucesso!");
        }
    }
}
