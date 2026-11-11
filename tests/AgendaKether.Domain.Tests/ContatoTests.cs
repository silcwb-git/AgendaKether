using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using FluentAssertions;

namespace AgendaKether.Domain.Tests;

public class ContatoTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarContato()
    {
        var contato = Contato.Criar("João Silva", "Rua A, 100", "(11) 98765-4321", "joao@kether.com");

        contato.Id.Should().NotBeEmpty();
        contato.Nome.Should().Be("João Silva");
        contato.Telefone.Numero.Should().Be("11987654321");
        contato.Email.Endereco.Should().Be("joao@kether.com");
        contato.Excluido.Should().BeFalse();
        contato.CriadoEm.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("J")]
    public void Criar_ComNomeInvalido_DeveLancarDomainException(string nome)
    {
        var acao = () => Contato.Criar(nome, "Rua A", "11987654321", "joao@kether.com");

        acao.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("semarroba")]
    [InlineData("@semdominio.com")]
    [InlineData("semlocal@")]
    [InlineData("a@b@c.com")]
    [InlineData("joao@dominio")]
    public void Criar_ComEmailInvalido_DeveLancarDomainException(string email)
    {
        var acao = () => Contato.Criar("João", "Rua A", "11987654321", email);

        acao.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("123")]           // 3 digitos - curto
    [InlineData("1234")]          // 4 digitos - curto
    [InlineData("011987654321")]  // 12 digitos - longo
    [InlineData("01987654321")]   // DDD comecando com 0
    [InlineData("11887654321")]   // 11 digitos, 3o digito = 8 (nao e celular)
    [InlineData("")]              // vazio
    public void Criar_ComTelefoneInvalido_DeveLancarDomainException(string telefone)
    {
        var acao = () => Contato.Criar("Joao", "Rua A", telefone, "joao@kether.com");

        acao.Should().Throw<DomainException>();
    }

    [Theory]
    [InlineData("11987654321")]   // celular: 11 digitos, 3o = 9
    [InlineData("1133334444")]    // fixo: 10 digitos
    public void Criar_ComTelefoneValido_DeveCriarContato(string telefone)
    {
        var contato = Contato.Criar("Joao", "Rua A", telefone, "joao@kether.com");

        contato.Telefone.Numero.Should().Be(telefone);
    }

    [Fact]
    public void Atualizar_ContatoExcluido_DeveLancarDomainException()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "joao@kether.com");
        contato.Excluir();

        var acao = () => contato.Atualizar("Maria", "Rua B", "11999998888", "maria@kether.com");

        acao.Should().Throw<DomainException>();
    }

    [Fact]
    public void Excluir_DuasVezes_DeveLancarDomainException()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "joao@kether.com");
        contato.Excluir();

        var acao = () => contato.Excluir();

        acao.Should().Throw<DomainException>();
    }

    [Fact]
    public void Email_DeveNormalizarParaMinusculo()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "JOAO@KETHER.COM");

        contato.Email.Endereco.Should().Be("joao@kether.com");
    }

    [Fact]
    public void Telefone_ComMascara_DeveGuardarApenasDigitos()
    {
        var contato = Contato.Criar("Joao", "Rua A", "(11) 98765-4321", "joao@kether.com");

        contato.Telefone.Numero.Should().Be("11987654321");
        contato.Telefone.Formatado().Should().Be("(11) 98765-4321");
    }
}