namespace AgendaKether.Application.Contatos.Commands.AtualizarContato;

public record AtualizarContatoCommand(Guid Id, string Nome, string Endereco, string Telefone, string Email);