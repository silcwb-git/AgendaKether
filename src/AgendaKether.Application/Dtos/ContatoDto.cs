namespace AgendaKether.Application.Dtos;

public record ContatoDto
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Endereco { get; init; } = string.Empty;
    public string Telefone { get; init; } = string.Empty;
    public string TelefoneFormatado { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CriadoEm { get; init; }
    public DateTime? AtualizadoEm { get; init; }
}