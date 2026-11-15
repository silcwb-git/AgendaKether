using AgendaKether.Application.Abstractions;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Contatos.Commands.AtualizarContato;

public class AtualizarContatoHandler : ICommandHandler<AtualizarContatoCommand, bool>
{
    private readonly IContatoRepository _repo;

    public AtualizarContatoHandler(IContatoRepository repo) => _repo = repo;

    public async Task<bool> Handle(AtualizarContatoCommand command, CancellationToken ct = default)
    {
        var contato = await _repo.ObterParaEdicao(command.Id, ct);

        if (contato is null)
            return false;

        var emailNormalizado = command.Email.Trim().ToLowerInvariant();

        if (await _repo.ExisteEmail(emailNormalizado, command.Id, ct))
            throw new DomainException("Ja existe outro contato com o e-mail '" + emailNormalizado + "'.");

        contato.Atualizar(command.Nome, command.Endereco, command.Telefone, emailNormalizado);

        await _repo.SalvarAlteracoes(ct);
        return true;
    }
}