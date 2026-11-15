using AgendaKether.Application.Abstractions;
using AgendaKether.Application.Dtos;
using AgendaKether.Domain.Repositorios;

namespace AgendaKether.Application.Contatos.Queries.ObterContatoPorId;

public class ObterContatoPorIdHandler : IQueryHandler<ObterContatoPorIdQuery, ContatoDto?>
{
    private readonly IContatoLeitura _leitura;

    public ObterContatoPorIdHandler(IContatoLeitura leitura) => _leitura = leitura;

    public async Task<ContatoDto?> Handle(ObterContatoPorIdQuery query, CancellationToken ct = default)
    {
        var contato = await _leitura.ObterPorId(query.Id, ct);

        if (contato is null)
            return null;

        return new ContatoDto
        {
            Id = contato.Id,
            Nome = contato.Nome,
            Endereco = contato.Endereco,
            Telefone = contato.Telefone.Numero,
            TelefoneFormatado = contato.Telefone.Formatado(),
            Email = contato.Email.Endereco,
            CriadoEm = contato.CriadoEm,
            AtualizadoEm = contato.AtualizadoEm
        };
    }
}