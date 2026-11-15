using AgendaKether.Application.Abstractions;
using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Contatos.Commands.CriarContato;

public class CriarContatoHandler : ICommandHandler<CriarContatoCommand, Guid>
{
    private readonly IContatoRepository _repo;

    public CriarContatoHandler(IContatoRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CriarContatoCommand command, CancellationToken ct = default)
    {
        var emailNormalizado = command.Email.Trim().ToLowerInvariant();

        if (await _repo.ExisteEmail(emailNormalizado, null, ct))
            throw new DomainException("Ja existe um contato com o e-mail '" + emailNormalizado + "'.");

        var contato = Contato.Criar(
            command.Nome,
            command.Endereco,
            command.Telefone,
            emailNormalizado);

        await _repo.Adicionar(contato, ct);
        await _repo.SalvarAlteracoes(ct);

        return contato.Id;
    }
}