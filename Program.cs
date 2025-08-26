using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;
using GerenciadorHotel.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Bind a porta do ambiente (Railway define PORT)
var portEnv = Environment.GetEnvironmentVariable("PORT");
if (int.TryParse(portEnv, out var port))
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(port);
    });
}

// Add services to the container.
// Monta a connection string a partir das variáveis do Railway, se existirem
string BuildConnectionString()
{
    // Primeiro tenta usar a MYSQL_URL do Railway (mais direto)
    var mysqlUrl = Environment.GetEnvironmentVariable("MYSQL_URL");
    if (!string.IsNullOrWhiteSpace(mysqlUrl))
    {
        // Railway fornece: mysql://user:password@host:port/database
        // Converte para format MySQL .NET: Server=host;Port=port;Database=db;Uid=user;Pwd=pwd;SslMode=Preferred;
        try
        {
            var uri = new Uri(mysqlUrl);
            var hostName = uri.Host;
            var portNumber = uri.Port > 0 ? uri.Port.ToString() : "3306";
            var databaseName = uri.PathAndQuery.TrimStart('/');
            var userName = uri.UserInfo.Split(':')[0];
            var userPassword = uri.UserInfo.Split(':')[1];
            
            return $"Server={hostName};Port={portNumber};Database={databaseName};Uid={userName};Pwd={userPassword};SslMode=Preferred;";
        }
        catch { /* fallback para variáveis individuais */ }
    }
    
    // Fallback: tenta variáveis individuais do Railway
    var host = Environment.GetEnvironmentVariable("MYSQLHOST");
    var db = Environment.GetEnvironmentVariable("MYSQLDATABASE");
    var user = Environment.GetEnvironmentVariable("MYSQLUSER");
    var pwd = Environment.GetEnvironmentVariable("MYSQLPASSWORD");
    var portVar = Environment.GetEnvironmentVariable("MYSQLPORT");
    if (!string.IsNullOrWhiteSpace(host) && !string.IsNullOrWhiteSpace(db) && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pwd))
    {
        var mysqlPort = string.IsNullOrWhiteSpace(portVar) ? "3306" : portVar;
        return $"Server={host};Port={mysqlPort};Database={db};Uid={user};Pwd={pwd};SslMode=Preferred;";
    }
    // fallback para appsettings
    return builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

var connectionString = BuildConnectionString();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Simplificar para desenvolvimento
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>() // Adicionar suporte a roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configurar autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("AdminOrReceptionist", policy => policy.RequireRole("Administrador", "Recepcionista"));
});

builder.Services.AddControllersWithViews();

// Registrar serviços personalizados
builder.Services.AddScoped<IEmpresaService, EmpresaService>();

var app = builder.Build();

// Aplicar headers de proxy (X-Forwarded-*) quando atrás de proxy (Railway)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Seed de dados inicial e migrações
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Aplica migrações automaticamente (útil em produção no Railway)
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        await SeedDataService.SeedRolesAndAdminUser(services);
    await SeedDataService.SeedPaises(services);
    await SeedDataService.SeedAmenidades(services);
    await SeedDataService.SeedEmpresaBase(services);
    // Primeiro tenta restaurar de backup se existir (não sobrescreve dados existentes)
    await SeedDataService.ImportarAcomodacoesDeBackup(services);
    // Garante as acomodações padrão caso não existam
    await SeedDataService.LimparEInserirAcomodacoesQuintaYpua(services);
    await SeedDataService.SeedAcomodacaoAmenidades(services);
    // Garante imagem principal para acomodações conhecidas sem foto
    await SeedDataService.GarantirImagemPrincipalAcomodacoes(services);
        await SeedDataService.AtualizarQuantidadeMaximaHospedes(services);
        await SeedDataService.CriarImagensAcomodacoes(services.GetRequiredService<ApplicationDbContext>());
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao executar seed de dados inicial");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Adicionar autenticação
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();