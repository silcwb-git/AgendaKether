using AgendaKether.Domain.Entidades;
using AgendaKether.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaKether.Infrastructure.Persistencia.Configuracoes;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();

        builder.Property(u => u.Nome)
               .IsRequired()
               .HasMaxLength(Usuario.NomeTamanhoMaximo);

        builder.Property(u => u.SenhaHash)
               .IsRequired()
               .HasMaxLength(Usuario.SenhaHashTamanhoMaximo);

        builder.Property(u => u.Excluido).IsRequired();
        builder.Property(u => u.CriadoEm).IsRequired();
        builder.Property(u => u.AtualizadoEm);

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Endereco)
                 .HasColumnName("Email")
                 .IsRequired()
                 .HasMaxLength(Email.TamanhoMaximo);

            email.HasIndex(e => e.Endereco)
                 .IsUnique()
                 .HasDatabaseName("IX_Usuarios_Email_Unique");
        });

        builder.Navigation(u => u.Email).IsRequired();
    }
}