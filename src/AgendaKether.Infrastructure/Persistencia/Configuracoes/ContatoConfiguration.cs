using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaKether.Infrastructure.Persistencia.Configuracoes;

public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
{
    public void Configure(EntityTypeBuilder<Contato> builder)
    {
        builder.ToTable("Contatos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedNever();

        builder.Property(c => c.Nome)
               .IsRequired()
               .HasMaxLength(Contato.NomeTamanhoMaximo);

        builder.Property(c => c.Endereco)
               .IsRequired()
               .HasMaxLength(Contato.EnderecoTamanhoMaximo);

        builder.Property(c => c.Excluido)
               .IsRequired();

        builder.Property(c => c.CriadoEm)
               .IsRequired();

        builder.Property(c => c.AtualizadoEm);

        // Value Object Email -> colunas na mesma tabela
        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Endereco)
                 .HasColumnName("Email")
                 .IsRequired()
                 .HasMaxLength(Email.TamanhoMaximo);

            email.HasIndex(e => e.Endereco)
                 .IsUnique()
                 .HasDatabaseName("IX_Contatos_Email_Unique");
        });

        // Value Object Telefone -> colunas na mesma tabela
        builder.OwnsOne(c => c.Telefone, tel =>
        {
            tel.Property(t => t.Numero)
               .HasColumnName("Telefone")
               .IsRequired()
               .HasMaxLength(11);
        });

        // O EF precisa saber que email.Endereco é acessível com private set
        builder.Navigation(c => c.Email).IsRequired();
        builder.Navigation(c => c.Telefone).IsRequired();

        // Índice para busca por nome (LIKE '%x%' não usa índice, mas ordenação usa)
        builder.HasIndex(c => c.Nome)
               .HasDatabaseName("IX_Contatos_Nome");
    }
}