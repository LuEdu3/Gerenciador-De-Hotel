using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Data;
using GerenciadorHotel.Models;
using GerenciadorHotel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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

// Seed de dados inicial
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedDataService.SeedRolesAndAdminUser(services);
        await SeedDataService.SeedPaises(services);
        await SeedDataService.SeedAmenidades(services);
    await SeedDataService.SeedEmpresaBase(services);
        await SeedDataService.LimparEInserirAcomodacoesQuintaYpua(services);
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