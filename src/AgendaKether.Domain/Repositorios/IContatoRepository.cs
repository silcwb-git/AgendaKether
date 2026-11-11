using AgendaKether.Domain.Entidades;

namespace AgendaKether.Domain.Repositorios
{
    public interface IContatoLeitura 
    {
        Task<Contato?> ObterPorId (Guid id, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<Contato> Itens, int Total)> Listar(
            string? busca,
            int pagina,
            int tamanhoPagina,
            CancellationToken ct = default);
        
       Task<bool> ExisteEmail(string email, Guid? ignorarId = null,   CancellationToken ct = default);
    }

    public interface IContatoEscrita
    {
        Task Adicionar(Contato contato, CancellationToken ct = default);
     
        Task<Contato?> ObterParaEdicao(Guid id, CancellationToken ct = default);
 
    }

    public interface IContatoRepositry : IContatoLeitura, IContatoEscrita
    {
        Task SalvarAlteracoes(CancellationToken ct = default);
    }
}