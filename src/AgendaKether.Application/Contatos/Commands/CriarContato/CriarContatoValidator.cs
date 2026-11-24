using FluentValidation;
using AgendaKether.Domain.Entidades;

namespace AgendaKether.Application.Contatos.Commands.CriarContato;

public class CriarContatoValidator : AbstractValidator<CriarContatoCommand>
{
    public CriarContatoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(Contato.NomeTamanhoMinimo)
                .WithMessage($"Nome deve ter no mínimo {Contato.NomeTamanhoMinimo} caracteres.")
            .MaximumLength(Contato.NomeTamanhoMaximo)
                .WithMessage($"Nome deve ter no máximo {Contato.NomeTamanhoMaximo} caracteres.");

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("Endereço é obrigatório.")
            .MaximumLength(Contato.EnderecoTamanhoMaximo)
                .WithMessage($"Endereço deve ter no máximo {Contato.EnderecoTamanhoMaximo} caracteres.");

        RuleFor(x => x.Telefone).NotEmpty().WithMessage("Telefone é obrigatório.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail é obrigatório.");
    }
}