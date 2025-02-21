using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.UseCases.Books.Filter;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [HttpGet]
    [Route("Filter")]
    [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
    public IActionResult Filter(int pageNumber, string? title)
    {
        var useCase = new FilterBookUseCase();

        var result = useCase.Execute(new RequestFilterBooksJson
        {
            PageNumber = pageNumber,
            Title = title
        });

        return Ok(result);
    }
}