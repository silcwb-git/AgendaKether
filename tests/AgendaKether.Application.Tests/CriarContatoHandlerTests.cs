using AgendaKether.Application.Contatos.Commands.CriarContato;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;
using FluentAssertions;
using NSubstitute;

namespace AgendaKether.Application.Tests;

public class CriarContatoHandlerTests
{
    private readonly IContatoRepository _repo = Substitute.For<IContatoRepository>();
    private readonly CriarContatoHandler _sut;

    public CriarContatoHandlerTests() => _sut = new CriarContatoHandler(_repo);

    [Fact]
    public async Task Handle_ComEmailNovo_DeveCriarESalvar()
    {
        _repo.ExisteEmail(Arg.Any<string>(), null, Arg.Any<CancellationToken>())
             .Returns(false);

        var command = new CriarContatoCommand("João", "Rua A", "11987654321", "joao@kether.com");

        var id = await _sut.Handle(command);

        id.Should().NotBeEmpty();
        await _repo.Received(1).Adicionar(Arg.Any<Domain.Entidades.Contato>(), Arg.Any<CancellationToken>());
        await _repo.Received(1).SalvarAlteracoes(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ComEmailJaExistente_DeveLancarDomainException()
    {
        _repo.ExisteEmail(Arg.Any<string>(), null, Arg.Any<CancellationToken>())
             .Returns(true);

        var command = new CriarContatoCommand("João", "Rua A", "11987654321", "joao@kether.com");

        var acao = () => _sut.Handle(command);

        await acao.Should().ThrowAsync<DomainException>();
        await _repo.DidNotReceive().Adicionar(Arg.Any<Domain.Entidades.Contato>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DeveNormalizarEmailAntesDeChecar()
    {
        _repo.ExisteEmail(Arg.Any<string>(), null, Arg.Any<CancellationToken>()).Returns(false);

        var command = new CriarContatoCommand("João", "Rua A", "11987654321", "JOAO@KETHER.COM");

        await _sut.Handle(command);

        await _repo.Received(1).ExisteEmail("joao@kether.com", null, Arg.Any<CancellationToken>());
    }
}