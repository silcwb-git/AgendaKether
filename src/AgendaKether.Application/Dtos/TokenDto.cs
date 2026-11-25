namespace AgendaKether.Application.Dtos;

public record TokenDto(string AccessToken, string Tipo, DateTime ExpiraEm, string Nome);