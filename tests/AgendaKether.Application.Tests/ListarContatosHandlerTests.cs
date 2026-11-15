using AgendaKether.Application.Contatos.Queries.ListarContatos;
using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Repositorios;
using FluentAssertions;
using NSubstitute;

namespace AgendaKether.Application.Tests;

public class ListarContatosHandlerTests
{
    private readonly IContatoLeitura _leitura = Substitute.For<IContatoLeitura>();
    private readonly ListarContatosHandler _sut;

    public ListarContatosHandlerTests() => _sut = new ListarContatosHandler(_leitura);

    [Fact]
    public async Task Handle_ComPaginaZerada_DeveCorrigirPara1()
    {
        _leitura.Listar(Arg.Any<string?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns((Array.Empty<Contato>(), 0));

        var resultado = await _sut.Handle(new ListarContatosQuery(null, 0, 10));

        resultado.Pagina.Should().Be(1);
        await _leitura.Received(1).Listar(null, 1, 10, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ComTamanhoAcimaDe100_DeveCorrigirPara10()
    {
        _leitura.Listar(Arg.Any<string?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns((Array.Empty<Contato>(), 0));

        var resultado = await _sut.Handle(new ListarContatosQuery(null, 1, 999999));

        resultado.TamanhoPagina.Should().Be(10);
    }

    [Fact]
    public async Task Handle_DeveCalcularTotalPaginasComCeiling()
    {
        var contato = Contato.Criar("João", "Rua A", "11987654321", "joao@kether.com");
        _leitura.Listar(Arg.Any<string?>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns((new[] { contato }, 25));

        var resultado = await _sut.Handle(new ListarContatosQuery(null, 1, 10));

        resultado.TotalPaginas.Should().Be(3);
    }
}