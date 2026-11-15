using AgendaKether.Application.Dtos;

namespace AgendaKether.Application.Contatos.Queries.ListarContatos;

public record ListarContatosResult(
    IReadOnlyList<ContatoDto> Itens,
    int Total,
    int Pagina,
    int TamanhoPagina,
    int TotalPaginas);