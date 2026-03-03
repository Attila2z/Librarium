using Librarium.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Librarium.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly LibrariumDbContext _db;

    public BooksController(LibrariumDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _db.Books.ToListAsync();
        return Ok(books.Select(b => new
        {
            bookId = b.Id,
            b.Title,
            isbn = b.Isbn,
            b.PublicationYear
        }));
    }
}
