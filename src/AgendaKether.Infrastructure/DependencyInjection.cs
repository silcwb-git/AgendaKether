using AgendaKether.Domain.Repositorios;
using AgendaKether.Infrastructure.Persistencia;
using AgendaKether.Infrastructure.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaKether.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AgendaKether")
            ?? "Data Source=agendakether.db";

        services.AddDbContext<AgendaKetherDbContext>(options =>
            options.UseSqlite(connectionString));

        // Uma única instância concreta por request
        services.AddScoped<ContatoRepository>();

        // As três interfaces apontam para a MESMA instância
        services.AddScoped<IContatoRepository>(sp => sp.GetRequiredService<ContatoRepository>());
        services.AddScoped<IContatoLeitura>(sp => sp.GetRequiredService<ContatoRepository>());
        services.AddScoped<IContatoEscrita>(sp => sp.GetRequiredService<ContatoRepository>());

        return services;
    }
}