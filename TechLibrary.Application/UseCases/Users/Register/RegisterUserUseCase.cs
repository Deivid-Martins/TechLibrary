using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Application.Infraestructure.DataAccess;
using TechLibrary.Application.Infraestructure.Security.Cryptography;
using TechLibrary.Application.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Application.UseCases.Users.Register;

public class RegisterUserUseCase
{
    public ResponseRegisteredUserJson Execute(RequestUserJson request)
    {
        var dbContext = new TechLibraryDbContext();

        Validate(request, dbContext);

        var cryptography = new BCryptAlgorithm();

        var entity = new User
        {
            Email = request.Email,
            Name = request.Name,
            Password = cryptography.HashPassword(request.Password),
        };

        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        var tokenGenerator = new JwtTokenGenerator();

        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            AccessToken = tokenGenerator.Generate(entity),
        };
    }

    private void Validate(RequestUserJson request, TechLibraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);

        var existUserWithSameEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));
        if (existUserWithSameEmail)
            result.Errors.Add(new ValidationFailure("Email", "E=mail ja Registrado na plataforma"));

        if (result.IsValid == false)
        {
            var errorsMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorsMessages);
        }
    }
}
