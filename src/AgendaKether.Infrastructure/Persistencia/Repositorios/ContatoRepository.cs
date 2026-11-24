using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace AgendaKether.Infrastructure.Persistencia.Repositorios;

public class ContatoRepository : IContatoRepository
{
    private readonly AgendaKetherDbContext _db;

    public ContatoRepository(AgendaKetherDbContext db) => _db = db;

    public async Task Adicionar(Contato contato, CancellationToken ct = default)
    {
        await _db.Contatos.AddAsync(contato, ct);
    }

    public async Task<Contato?> ObterPorId(Guid id, CancellationToken ct = default)
    {
        return await _db.Contatos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && !c.Excluido, ct);
    }

    public async Task<Contato?> ObterParaEdicao(Guid id, CancellationToken ct = default)
    {
        return await _db.Contatos
            .FirstOrDefaultAsync(c => c.Id == id && !c.Excluido, ct);
    }

    public async Task<(IReadOnlyList<Contato> Itens, int Total)> Listar(
        string? busca, int pagina, int tamanhoPagina, CancellationToken ct = default)
    {
        var query = _db.Contatos
            .AsNoTracking()
            .Where(c => !c.Excluido);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.Nome, $"%{termo}%") ||
                EF.Functions.Like(c.Email.Endereco, $"%{termo}%") ||
                EF.Functions.Like(c.Telefone.Numero, $"%{termo}%"));
        }

        var total = await query.CountAsync(ct);

        var itens = await query
            .OrderBy(c => c.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(ct);

        return (itens, total);
    }

    public async Task<bool> ExisteEmail(string email, Guid? ignorarId = default, CancellationToken ct = default)
    {
        var query = _db.Contatos
            .AsNoTracking()
            .Where(c => c.Email.Endereco == email && !c.Excluido);

        if (ignorarId.HasValue)
            query = query.Where(c => c.Id != ignorarId.Value);

        return await query.AnyAsync(ct);
    }

    public async Task SalvarAlteracoes(CancellationToken ct = default)
    {
        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (EhViolacaoDeUnicidade(ex))
        {
            // Traduz o erro técnico do banco em regra de negócio.
            // A Api não precisa (nem deve) conhecer DbUpdateException.
            throw new DomainException("Já existe um contato com este e-mail.");
        }
    }

    private static bool EhViolacaoDeUnicidade(DbUpdateException ex)
    {
        var mensagem = ex.InnerException?.Message;
        if (string.IsNullOrEmpty(mensagem))
            return false;

        return mensagem.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase)
            || mensagem.Contains("duplicate key", StringComparison.OrdinalIgnoreCase);
    }
}