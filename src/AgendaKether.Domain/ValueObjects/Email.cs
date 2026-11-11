using AgendaKether.Domain.Excecoes;

namespace AgendaKether.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    public const int TamanhoMaximo = 254;

    public string Endereco { get; }

    private Email(string endereco) => Endereco = endereco;

    public static Email Criar(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
            throw new DomainException("E-mail é obrigatório.");

        endereco = endereco.Trim().ToLowerInvariant();

        var partes = endereco.Split('@');

        if (partes.Length != 2 || partes[0].Length == 0 || partes[1].Length == 0)
            throw new DomainException("E-mail inválido: '" + endereco + "'.");

        if (!partes[1].Contains('.') || partes[1].StartsWith('.') || partes[1].EndsWith('.'))
            throw new DomainException("Domínio de e-mail inválido: '" + partes[1] + "'.");

        if (endereco.Length > TamanhoMaximo)
            throw new DomainException("E-mail não pode passar de " + TamanhoMaximo + " caracteres.");

        return new Email(endereco);
    }

    public bool Equals(Email? outro) => outro is not null && Endereco == outro.Endereco;
    public override bool Equals(object? obj) => Equals(obj as Email);
    public override int GetHashCode() => Endereco.GetHashCode();
    public override string ToString() => Endereco;
}