using AgendaKether.Domain.Entidades;
using AgendaKether.Infrastructure.Persistencia;
using AgendaKether.Infrastructure.Persistencia.Repositorios;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AgendaKether.Infrastructure.Tests;

public class ContatoRepositoryTests : IDisposable
{
    private readonly SqliteConnection _conexao;
    private readonly AgendaKetherDbContext _db;
    private readonly ContatoRepository _sut;

    public ContatoRepositoryTests()
    {
        _conexao = new SqliteConnection("DataSource=:memory:");
        _conexao.Open(); // para o banco de dados em memória funcionar, a conexão precisa permanecer aberta durante o teste

        var options = new DbContextOptionsBuilder<AgendaKetherDbContext>()
            .UseSqlite(_conexao)
            .Options;

        _db = new AgendaKetherDbContext(options);
        _db.Database.EnsureCreated();

        _sut = new ContatoRepository(_db);
    }

    [Fact]
    public async Task AdicionarEDevolver_PersisteContato()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "joao@kether.com");

        await _sut.Adicionar(contato);
        await _sut.SalvarAlteracoes();

        var achado = await _sut.ObterPorId(contato.Id);

        achado.Should().NotBeNull();
        achado!.Nome.Should().Be("Joao");
        achado.Telefone.Numero.Should().Be("11987654321");
        achado.Telefone.Formatado().Should().Be("(11) 98765-4321");
        achado.Email.Endereco.Should().Be("joao@kether.com");
    }

    [Fact]
    public async Task ObterPorId_ComExcluido_DevolveNull()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "joao@kether.com");
        await _sut.Adicionar(contato);
        await _sut.SalvarAlteracoes();

        contato.Excluir();
        await _sut.SalvarAlteracoes();

        var achado = await _sut.ObterPorId(contato.Id);

        achado.Should().BeNull();
    }

    [Fact]
    public async Task ExisteEmail_ComEmailExistente_DevolveTrue()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "joao@kether.com");
        await _sut.Adicionar(contato);
        await _sut.SalvarAlteracoes();

        var existe = await _sut.ExisteEmail("joao@kether.com");

        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteEmail_ComIgnorarId_DevolveFalse()
    {
        var contato = Contato.Criar("Joao", "Rua A", "11987654321", "joao@kether.com");
        await _sut.Adicionar(contato);
        await _sut.SalvarAlteracoes();

        var existe = await _sut.ExisteEmail("joao@kether.com", contato.Id);

        existe.Should().BeFalse();
    }

    [Fact]
    public async Task Listar_ComPaginacao_DevolveTotalCorreto()
    {
        for (var i = 0; i < 25; i++)
        {
            var c = Contato.Criar("Contato " + i.ToString("D2"), "Rua", "11987654321", "c" + i + "@kether.com");
            await _sut.Adicionar(c);
        }
        await _sut.SalvarAlteracoes();

        var (itens, total) = await _sut.Listar(null, 1, 10);

        itens.Should().HaveCount(10);
        total.Should().Be(25);
    }

    [Fact]
    public async Task Listar_ComBuscaPorNome_FiltraCorretamente()
    {
        await _sut.Adicionar(Contato.Criar("Maria Silva", "Rua A", "11987654321", "maria@kether.com"));
        await _sut.Adicionar(Contato.Criar("Joao Souza", "Rua B", "11999998888", "joao@kether.com"));
        await _sut.SalvarAlteracoes();

        var (itens, total) = await _sut.Listar("Maria", 1, 10);

        itens.Should().HaveCount(1);
        total.Should().Be(1);
        itens[0].Nome.Should().Be("Maria Silva");
    }

    [Fact]
    public async Task Adicionar_ComEmailDuplicado_DeveFalharNoBanco()
    {
        var a = Contato.Criar("Joao", "Rua A", "11987654321", "mesmo@kether.com");
        var b = Contato.Criar("Maria", "Rua B", "11999998888", "mesmo@kether.com");

        await _sut.Adicionar(a);
        await _sut.SalvarAlteracoes();

        await _sut.Adicionar(b);
        var acao = () => _sut.SalvarAlteracoes();

        await acao.Should().ThrowAsync<DbUpdateException>();
    }

    public void Dispose()
    {
        _db.Dispose();
        _conexao.Dispose();
    }
}