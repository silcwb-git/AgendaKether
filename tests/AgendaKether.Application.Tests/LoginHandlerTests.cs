using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Auth.Commands.Login;
using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;
using FluentAssertions;
using NSubstitute;

namespace AgendaKether.Application.Tests;

public class LoginHandlerTests
{
    private const string HashValido = "$2a$11$abcdefghijklmnopqrstuvABCDEFGHIJKLMNOPQRSTUVWXYZ0123456";

    private readonly IUsuarioRepository _repo = Substitute.For<IUsuarioRepository>();
    private readonly ISenhaHasher _hasher = Substitute.For<ISenhaHasher>();
    private readonly ITokenService _token = Substitute.For<ITokenService>();
    private readonly LoginHandler _sut;

    public LoginHandlerTests() => _sut = new LoginHandler(_repo, _hasher, _token);

    [Fact]
    public async Task Handle_ComCredenciaisValidas_DeveGerarToken()
    {
        var usuario = Usuario.Criar("Silvana", "silvana@kether.com", HashValido);
        var expiraEm = DateTime.UtcNow.AddHours(1);

        _repo.ObterPorEmail("silvana@kether.com", Arg.Any<CancellationToken>()).Returns(usuario);
        _hasher.Verificar("Senha@123", HashValido).Returns(true);
        _token.Gerar(usuario).Returns(("token-jwt", expiraEm));

        var resultado = await _sut.Handle(new LoginCommand("silvana@kether.com", "Senha@123"));

        resultado.AccessToken.Should().Be("token-jwt");
        resultado.Tipo.Should().Be("Bearer");
        resultado.Nome.Should().Be("Silvana");
    }

    [Fact]
    public async Task Handle_ComUsuarioInexistente_DeveLancarDomainException()
    {
        _repo.ObterPorEmail(Arg.Any<string>(), Arg.Any<CancellationToken>())
             .Returns((Usuario?)null);

        var acao = () => _sut.Handle(new LoginCommand("naoexiste@kether.com", "Senha@123"));

        await acao.Should().ThrowAsync<DomainException>()
                  .WithMessage("E-mail ou senha inválidos.");
    }

    [Fact]
    public async Task Handle_ComSenhaErrada_DeveLancarDomainException()
    {
        var usuario = Usuario.Criar("Silvana", "silvana@kether.com", HashValido);

        _repo.ObterPorEmail(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(usuario);
        _hasher.Verificar("Errada@1", HashValido).Returns(false);

        var acao = () => _sut.Handle(new LoginCommand("silvana@kether.com", "Errada@1"));

        await acao.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_ComSenhaErrada_NaoDeveGerarToken()
    {
        var usuario = Usuario.Criar("Silvana", "silvana@kether.com", HashValido);

        _repo.ObterPorEmail(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(usuario);
        _hasher.Verificar(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        try { await _sut.Handle(new LoginCommand("silvana@kether.com", "Errada@1")); }
        catch (DomainException) { }

        _token.DidNotReceive().Gerar(Arg.Any<Usuario>());
    }
}
