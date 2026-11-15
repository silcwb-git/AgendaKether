using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Contatos.Commands.AtualizarContato;
using AgendaKether.Application.Contatos.Commands.CriarContato;
using AgendaKether.Application.Contatos.Commands.ExcluirContato;
using AgendaKether.Application.Contatos.Queries.ListarContatos;
using AgendaKether.Application.Contatos.Queries.ObterContatoPorId;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaKether.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CriarContatoCommand, Guid>, CriarContatoHandler>();
        services.AddScoped<ICommandHandler<AtualizarContatoCommand, bool>, AtualizarContatoHandler>();
        services.AddScoped<ICommandHandler<ExcluirContatoCommand, bool>, ExcluirContatoHandler>();

        services.AddScoped<IQueryHandler<ObterContatoPorIdQuery, Dtos.ContatoDto?>, ObterContatoPorIdHandler>();
        services.AddScoped<IQueryHandler<ListarContatosQuery, ListarContatosResult>, ListarContatosHandler>();

        return services;
    }
}