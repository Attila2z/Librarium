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
    var books = await _db.Books
        .AsNoTracking()
        .Where(b => !b.IsRetired)
        .Include(b => b.Authors)
        .ToListAsync();

    return Ok(books.Select(b => new
    {
        bookId = b.Id,
        title = b.Title,
        isbn = b.Isbn,
        publicationYear = b.PublicationYear,

        // NEW
        authors = b.Authors.Select(a => new
        {
            firstName = a.FirstName,
            lastName = a.LastName,
            biography = a.Biography
        }).ToList()
    }));
    }
}
