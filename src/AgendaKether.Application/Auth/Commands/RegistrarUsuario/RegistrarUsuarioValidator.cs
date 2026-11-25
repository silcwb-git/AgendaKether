using AgendaKether.Domain.Entidades;
using FluentValidation;

namespace AgendaKether.Application.Auth.Commands.RegistrarUsuario;

public class RegistrarUsuarioValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(Usuario.NomeTamanhoMinimo)
                .WithMessage($"Nome deve ter no mínimo {Usuario.NomeTamanhoMinimo} caracteres.")
            .MaximumLength(Usuario.NomeTamanhoMaximo)
                .WithMessage($"Nome deve ter no máximo {Usuario.NomeTamanhoMaximo} caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail em formato inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(Usuario.SenhaTamanhoMinimo)
                .WithMessage($"Senha deve ter no mínimo {Usuario.SenhaTamanhoMinimo} caracteres.")
            .MaximumLength(72)
                .WithMessage("Senha deve ter no máximo 72 caracteres.");
    }
}