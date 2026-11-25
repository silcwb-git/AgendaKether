using AgendaKether.Application.Abstractions;
using AgendaKether.Domain.Repositorios;
using AgendaKether.Infrastructure.Persistencia;
using AgendaKether.Infrastructure.Persistencia.Repositorios;
using AgendaKether.Infrastructure.Seguranca;
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

        // Contatos
        services.AddScoped<ContatoRepository>();
        services.AddScoped<IContatoRepository>(sp => sp.GetRequiredService<ContatoRepository>());
        services.AddScoped<IContatoLeitura>(sp => sp.GetRequiredService<ContatoRepository>());
        services.AddScoped<IContatoEscrita>(sp => sp.GetRequiredService<ContatoRepository>());

        // Usuários
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        // Segurança
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddSingleton<ISenhaHasher, BcryptSenhaHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}