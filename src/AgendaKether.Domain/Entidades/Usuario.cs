using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.ValueObjects;

namespace AgendaKether.Domain.Entidades;

public class Usuario
{
    public const int NomeTamanhoMinimo = 2;
    public const int NomeTamanhoMaximo = 120;
    public const int SenhaTamanhoMinimo = 8;
    public const int SenhaHashTamanhoMaximo = 200;

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public bool Excluido { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }

    private Usuario() { }

    public static Usuario Criar(string nome, string email, string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");

        var nomeLimpo = nome.Trim();

        if (nomeLimpo.Length < NomeTamanhoMinimo)
            throw new DomainException($"Nome deve ter no mínimo {NomeTamanhoMinimo} caracteres.");

        if (nomeLimpo.Length > NomeTamanhoMaximo)
            throw new DomainException($"Nome deve ter no máximo {NomeTamanhoMaximo} caracteres.");

        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new DomainException("Hash de senha é obrigatório.");

        return new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = nomeLimpo,
            Email = Email.Criar(email),
            SenhaHash = senhaHash.Trim(),
            Excluido = false,
            CriadoEm = DateTime.UtcNow
        };
    }

    public void Excluir()
    {
        if (Excluido)
            throw new DomainException("Usuário já está excluído.");

        Excluido = true;
        AtualizadoEm = DateTime.UtcNow;
    }
}