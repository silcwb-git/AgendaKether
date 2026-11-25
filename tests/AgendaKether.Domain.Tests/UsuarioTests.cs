using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using FluentAssertions;

namespace AgendaKether.Domain.Tests;

public class UsuarioTests
{
    private const string HashValido = "$2a$11$abcdefghijklmnopqrstuvABCDEFGHIJKLMNOPQRSTUVWXYZ0123456";

    [Fact]
    public void Criar_ComDadosValidos_DeveCriarUsuario()
    {
        var usuario = Usuario.Criar("Silvana", "silvana@kether.com", HashValido);

        usuario.Id.Should().NotBeEmpty();
        usuario.Nome.Should().Be("Silvana");
        usuario.Email.Endereco.Should().Be("silvana@kether.com");
        usuario.SenhaHash.Should().Be(HashValido);
        usuario.Excluido.Should().BeFalse();
        usuario.CriadoEm.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("S")]
    public void Criar_ComNomeInvalido_DeveLancarDomainException(string nome)
    {
        var acao = () => Usuario.Criar(nome, "silvana@kether.com", HashValido);

        acao.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_SemSenhaHash_DeveLancarDomainException(string hash)
    {
        var acao = () => Usuario.Criar("Silvana", "silvana@kether.com", hash);

        acao.Should().Throw<DomainException>();
    }

    [Fact]
    public void Excluir_DuasVezes_DeveLancarDomainException()
    {
        var usuario = Usuario.Criar("Silvana", "silvana@kether.com", HashValido);
        usuario.Excluir();

        var acao = () => usuario.Excluir();

        acao.Should().Throw<DomainException>();
    }
}
