using AgendaKether.Domain.Excecoes;

namespace AgendaKether.Domain.ValueObjects;

public sealed class Telefone : IEquatable<Telefone>
{
    public string Numero { get; }

    private Telefone(string numero) => Numero = numero;

    public static Telefone Criar(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new DomainException("Telefone é obrigatório.");

        var digitos = new string(numero.Where(char.IsDigit).ToArray());

        if (digitos.Length < 10 || digitos.Length > 11)
            throw new DomainException("Telefone deve ter 10 (fixo) ou 11 (celular) dígitos.");

        var ddd = digitos.Substring(0, 2);

        if (ddd[0] == '0')
            throw new DomainException("DDD inválido: '" + ddd + "'.");

        if (digitos.Length == 11 && digitos[2] != '9')
            throw new DomainException("Celular deve começar com 9 após o DDD.");

        return new Telefone(digitos);
    }

    public string Formatado()
    {
        if (Numero.Length == 11)
        {
            return "(" + Numero.Substring(0, 2) + ") "
                 + Numero.Substring(2, 5) + "-"
                 + Numero.Substring(7);
        }

        return "(" + Numero.Substring(0, 2) + ") "
             + Numero.Substring(2, 4) + "-"
             + Numero.Substring(6);
    }

    public bool Equals(Telefone? outro) => outro is not null && Numero == outro.Numero;
    public override bool Equals(object? obj) => Equals(obj as Telefone);
    public override int GetHashCode() => Numero.GetHashCode();
    public override string ToString() => Formatado();
}