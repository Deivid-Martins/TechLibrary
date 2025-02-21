using TechLibrary.Application.Infraestructure.DataAccess;
using TechLibrary.Application.Infraestructure.Security.Cryptography;
using TechLibrary.Application.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase
{
    public ResponseRegisteredUserJson Execute(RequestLoginJson request)
    {
        var dbContext = new TechLibraryDbContext();

        var user = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));
        if(user == null)
        {
            throw new InvalidLoginException();
        };

        var cryptography = new BCryptAlgorithm();
        var isPasswordValid = cryptography.Verify(request.Password, user);
        if(isPasswordValid == false)
            throw new InvalidLoginException();

        var tokenGenerator = new JwtTokenGenerator();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            AccessToken = tokenGenerator.Generate(user),
        };
    }
}
