using FluentValidation;
using AgendaKether.Domain.Entidades;

namespace AgendaKether.Application.Contatos.Commands.AtualizarContato;

public class AtualizarContatoValidator : AbstractValidator<AtualizarContatoCommand>
{
    public AtualizarContatoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(Contato.NomeTamanhoMinimo)
            .MaximumLength(Contato.NomeTamanhoMaximo);

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("Endereço é obrigatório.")
            .MaximumLength(Contato.EnderecoTamanhoMaximo);

        RuleFor(x => x.Telefone).NotEmpty().WithMessage("Telefone é obrigatório.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.");
    }
}