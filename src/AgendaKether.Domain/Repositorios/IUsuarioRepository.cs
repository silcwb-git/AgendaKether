using AgendaKether.Domain.Entidades;

namespace AgendaKether.Domain.Repositorios;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmail(string email, CancellationToken ct = default);
    Task<bool> ExisteEmail(string email, CancellationToken ct = default);
    Task Adicionar(Usuario usuario, CancellationToken ct = default);
    Task SalvarAlteracoes(CancellationToken ct = default);
}