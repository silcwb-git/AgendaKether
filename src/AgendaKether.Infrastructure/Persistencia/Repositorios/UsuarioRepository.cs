using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace AgendaKether.Infrastructure.Persistencia.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AgendaKetherDbContext _db;

    public UsuarioRepository(AgendaKetherDbContext db) => _db = db;

    public async Task<Usuario?> ObterPorEmail(string email, CancellationToken ct = default)
    {
        return await _db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Endereco == email && !u.Excluido, ct);
    }

    public async Task<bool> ExisteEmail(string email, CancellationToken ct = default)
    {
        return await _db.Usuarios
            .AsNoTracking()
            .AnyAsync(u => u.Email.Endereco == email && !u.Excluido, ct);
    }

    public async Task Adicionar(Usuario usuario, CancellationToken ct = default)
    {
        await _db.Usuarios.AddAsync(usuario, ct);
    }

    public async Task SalvarAlteracoes(CancellationToken ct = default)
    {
        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (EhViolacaoDeUnicidade(ex))
        {
            throw new DomainException("Já existe um usuário com este e-mail.");
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