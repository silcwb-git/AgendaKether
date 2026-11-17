using AgendaKether.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace AgendaKether.Infrastructure.Persistencia;

public class AgendaKetherDbContext : DbContext
{
    public AgendaKetherDbContext(DbContextOptions<AgendaKetherDbContext> options)
        : base(options) { }

    public DbSet<Contato> Contatos => Set<Contato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgendaKetherDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}