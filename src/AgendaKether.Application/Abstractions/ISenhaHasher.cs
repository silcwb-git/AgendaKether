namespace AgendaKether.Application.Abstractions;

public interface ISenhaHasher
{
    string Gerar(string senha);
    bool Verificar(string senha, string hashArmazenado);
}