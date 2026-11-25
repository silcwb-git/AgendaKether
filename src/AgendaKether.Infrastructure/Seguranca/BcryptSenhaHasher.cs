using AgendaKether.Application.Abstractions;

namespace AgendaKether.Infrastructure.Seguranca;

public class BcryptSenhaHasher : ISenhaHasher
{
    // workFactor 11 = ~100ms por hash em hardware comum.
    // Lento o bastante para inviabilizar força bruta,
    // rápido o bastante para não travar o login.
    private const int WorkFactor = 11;

    public string Gerar(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: WorkFactor);
    }

    public bool Verificar(string senha, string hashArmazenado)
    {
        if (string.IsNullOrWhiteSpace(hashArmazenado))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(senha, hashArmazenado);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Hash corrompido ou em formato desconhecido: trata como senha errada,
            // nunca deixa a exceção vazar como 500.
            return false;
        }
    }
}