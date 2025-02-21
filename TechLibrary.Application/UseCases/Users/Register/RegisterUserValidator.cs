using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("O Nome é obrigatório.");
        RuleFor(request => request.Email).EmailAddress().WithMessage("O Email não é válido.");
        RuleFor(request => request.Password).NotEmpty().WithMessage("A senha é obrigatória.");
        When(request => string.IsNullOrEmpty(request.Password) == false, () =>
        {
            RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6).WithMessage("Senha deve ser maior que 6 caracteres.");
        });
    }
}
