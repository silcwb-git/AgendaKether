using AgendaKether.Application.Abstractions;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Contatos.Commands.ExcluirContato;

public class ExcluirContatoHandler : ICommandHandler<ExcluirContatoCommand, bool>
{
    private readonly IContatoRepository _repo;

    public ExcluirContatoHandler(IContatoRepository repo) => _repo = repo;

    public async Task<bool> Handle(ExcluirContatoCommand command, CancellationToken ct = default)
    {
        var contato = await _repo.ObterParaEdicao(command.Id, ct);

        if (contato is null)
            return false;

        contato.Excluir();

        await _repo.SalvarAlteracoes(ct);
        return true;
    }
}