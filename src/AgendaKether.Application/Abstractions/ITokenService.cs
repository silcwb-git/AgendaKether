using AgendaKether.Domain.Entidades;

namespace AgendaKether.Application.Abstractions;

public interface ITokenService
{
    (string Token, DateTime ExpiraEm) Gerar(Usuario usuario);
}