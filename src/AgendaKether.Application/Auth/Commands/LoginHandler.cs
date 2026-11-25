using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Dtos;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Auth.Commands.Login;

public class LoginHandler : ICommandHandler<LoginCommand, TokenDto>
{
    private readonly IUsuarioRepository _repo;
    private readonly ISenhaHasher _hasher;
    private readonly ITokenService _token;

    public LoginHandler(IUsuarioRepository repo, ISenhaHasher hasher, ITokenService token)
    {
        _repo = repo;
        _hasher = hasher;
        _token = token;
    }

    public async Task<TokenDto> Handle(LoginCommand command, CancellationToken ct = default)
    {
        var email = command.Email.Trim().ToLowerInvariant();
        var usuario = await _repo.ObterPorEmail(email, ct);

        // Mesma mensagem para e-mail inexistente e senha errada.
        // Se dissesse "e-mail não encontrado", você entregaria a lista de
        // usuários válidos para quem quisesse atacar.
        if (usuario is null || !_hasher.Verificar(command.Senha, usuario.SenhaHash))
            throw new DomainException("E-mail ou senha inválidos.");

        var (token, expiraEm) = _token.Gerar(usuario);

        return new TokenDto(token, "Bearer", expiraEm, usuario.Nome);
    }
}