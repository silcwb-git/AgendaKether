using AgendaKether.Application.Abstractions;
using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Auth.Commands.RegistrarUsuario;

public class RegistrarUsuarioHandler : ICommandHandler<RegistrarUsuarioCommand, Guid>
{
    private readonly IUsuarioRepository _repo;
    private readonly ISenhaHasher _hasher;

    public RegistrarUsuarioHandler(IUsuarioRepository repo, ISenhaHasher hasher)
    {
        _repo = repo;
        _hasher = hasher;
    }

    public async Task<Guid> Handle(RegistrarUsuarioCommand command, CancellationToken ct = default)
    {
        var email = command.Email.Trim().ToLowerInvariant();

        if (await _repo.ExisteEmail(email, ct))
            throw new DomainException($"Já existe um usuário com o e-mail '{email}'.");

        var hash = _hasher.Gerar(command.Senha);
        var usuario = Usuario.Criar(command.Nome, email, hash);

        await _repo.Adicionar(usuario, ct);
        await _repo.SalvarAlteracoes(ct);

        return usuario.Id;
    }
}