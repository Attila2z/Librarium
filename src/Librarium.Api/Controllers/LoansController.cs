using Librarium.Data;
using Librarium.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Librarium.Api.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    private readonly LibrariumDbContext _db;

    public LoansController(LibrariumDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanRequest request)
    {
        var loan = new Loan
        {
            MemberId = request.MemberId,
            BookId = request.BookId,
            LoanDate = request.LoanDate,
            ReturnDate = request.ReturnDate
        };

        _db.Loans.Add(loan);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLoans), new { memberId = request.MemberId }, loan);
    }

    [HttpGet("{memberId}")]
    public async Task<IActionResult> GetLoans(int memberId)
    {
        var loans = await _db.Loans
            .Where(l => l.MemberId == memberId)
            .Include(l => l.Book)
            .ToListAsync();

        return Ok(loans.Select(l => new
        {
            loanId = l.Id,
            bookTitle = l.Book.Title,
            l.LoanDate,
            l.ReturnDate,
            status = l.Status.ToString()
        }));
    }
}

public class CreateLoanRequest
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
