using System.Net;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Application.Domain.Entities;
using TechLibrary.Application.Infraestructure.DataAccess;
using TechLibrary.Exception;

namespace TechLibrary.Application.UseCases.Checkouts;

public class RegisterBookCheckoutUseCase
{
    private const int MAX_LOAN_DAYS = 7;

    private readonly LoggedUserService _loggedUser;

    public RegisterBookCheckoutUseCase(LoggedUserService loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();

        Validate(dbContext, bookId);

        var user = _loggedUser.User(dbContext);

        var entity = new Checkout
        {
            UserId = user.Id,
            BookId = bookId,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS),
        };

        dbContext.Add(entity);

        dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);
        if (book == null)
            throw new NotFoundException("Livro não encontrado.");

        var amountBookNotReturned = dbContext
            .Checkouts
            .Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        if(amountBookNotReturned == book.Amount)
            throw new NotFoundException("Livro não está disponível para empréstimo.");
    }
}
