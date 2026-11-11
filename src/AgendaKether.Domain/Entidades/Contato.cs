using AgendaKether.Domain.Excecoes;
using AgendaKether.Domain.ValueObjects;

namespace AgendaKether.Domain.Entidades;

public class Contato
{
    public const int NomeTamanhoMinimo = 2;
    public const int NomeTamanhoMaximo = 120;
    public const int EnderecoTamanhoMaximo = 250;

    public Guid Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Endereco { get; private set; } = null!;
    public Telefone Telefone { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public bool Excluido { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }

    private Contato() { }

    public static Contato Criar(string nome, string endereco, string telefone, string email)
    {
        var contato = new Contato
        {
            Id = Guid.NewGuid(),
            Telefone = ValueObjects.Telefone.Criar(telefone),
            Email = ValueObjects.Email.Criar(email),
            Excluido = false,
            CriadoEm = DateTime.UtcNow
        };

        contato.DefinirNome(nome);
        contato.DefinirEndereco(endereco);

        return contato;
    }

    public void Atualizar(string nome, string endereco, string telefone, string email)
    {
        if (Excluido)
            throw new DomainException("Contato excluído não pode ser alterado.");

        DefinirNome(nome);
        DefinirEndereco(endereco);
        Telefone = ValueObjects.Telefone.Criar(telefone);
        Email = ValueObjects.Email.Criar(email);
        AtualizadoEm = DateTime.UtcNow;
    }

    public void Excluir()
    {
        if (Excluido)
            throw new DomainException("Contato já está excluído.");

        Excluido = true;
        AtualizadoEm = DateTime.UtcNow;
    }

    private void DefinirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");

        nome = nome.Trim();

        if (nome.Length < NomeTamanhoMinimo)
            throw new DomainException("Nome deve ter ao menos " + NomeTamanhoMinimo + " caracteres.");

        if (nome.Length > NomeTamanhoMaximo)
            throw new DomainException("Nome não pode passar de " + NomeTamanhoMaximo + " caracteres.");

        Nome = nome;
    }

    private void DefinirEndereco(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
            throw new DomainException("Endereço é obrigatório.");

        endereco = endereco.Trim();

        if (endereco.Length > EnderecoTamanhoMaximo)
            throw new DomainException("Endereço não pode passar de " + EnderecoTamanhoMaximo + " caracteres.");

        Endereco = endereco;
    }
}