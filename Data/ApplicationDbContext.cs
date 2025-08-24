using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Models;

namespace GerenciadorHotel.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets para as entidades do sistema
    public DbSet<Pais> Paises { get; set; }
    public DbSet<Amenidade> Amenidades { get; set; }
    public DbSet<Acomodacao> Acomodacoes { get; set; }
    public DbSet<AcomodacaoAmenidade> AcomodacaoAmenidades { get; set; }
    public DbSet<ImagemAcomodacao> ImagensAcomodacao { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<EmpresaFoto> EmpresaFotos { get; set; }
    public DbSet<EmpresaServico> EmpresaServicos { get; set; } = null!;
    public DbSet<EmpresaPremio> EmpresaPremios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurações de relacionamentos e índices

        // Relacionamento Many-to-Many entre Acomodacao e Amenidade
        builder.Entity<AcomodacaoAmenidade>()
            .HasIndex(aa => new { aa.AcomodacaoId, aa.AmenidadeId })
            .IsUnique();

        // Configurações de precisão para campos decimais
        builder.Entity<Acomodacao>()
            .Property(a => a.Preco)
            .HasPrecision(10, 2);

        builder.Entity<Reserva>()
            .Property(r => r.ValorTotal)
            .HasPrecision(10, 2);

        builder.Entity<Pagamento>()
            .Property(p => p.Valor)
            .HasPrecision(10, 2);

        // Índices para melhorar performance
        builder.Entity<Reserva>()
            .HasIndex(r => r.DataCheckIn);

        builder.Entity<Reserva>()
            .HasIndex(r => r.DataCheckOut);

        builder.Entity<Reserva>()
            .HasIndex(r => r.Email);

        builder.Entity<Acomodacao>()
            .HasIndex(a => a.Status);

        // Configurações para ApplicationUser
        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.NivelAcesso);

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.Ativo);

        // Seed data inicial pode ser adicionado aqui no futuro
    }
}
