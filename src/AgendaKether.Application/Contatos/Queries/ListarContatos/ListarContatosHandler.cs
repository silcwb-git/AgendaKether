using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Dtos;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Contatos.Queries.ListarContatos;

public class ListarContatosHandler : IQueryHandler<ListarContatosQuery, ListarContatosResult>
{
    private readonly IContatoLeitura _leitura;

    public ListarContatosHandler(IContatoLeitura leitura) => _leitura = leitura;

    public async Task<ListarContatosResult> Handle(ListarContatosQuery query, CancellationToken ct = default)
    {
        var pagina = query.Pagina < 1 ? 1 : query.Pagina;
        var tamanho = query.TamanhoPagina is < 1 or > 100 ? 10 : query.TamanhoPagina;

        var (itens, total) = await _leitura.Listar(query.Busca, pagina, tamanho, ct);

        var dtos = itens.Select(c => new ContatoDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Endereco = c.Endereco,
            Telefone = c.Telefone.Numero,
            TelefoneFormatado = c.Telefone.Formatado(),
            Email = c.Email.Endereco,
            CriadoEm = c.CriadoEm,
            AtualizadoEm = c.AtualizadoEm
        }).ToList();

        var totalPaginas = (int)Math.Ceiling(total / (double)tamanho);

        return new ListarContatosResult(dtos, total, pagina, tamanho, totalPaginas);
    }
}