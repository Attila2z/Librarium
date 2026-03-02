namespace Librarium.Data.Entities;

public class Loan
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    // Using DateTimeOffset -> maps nicely to Postgres timestamptz
    public DateTimeOffset LoanDate { get; set; }

    public DateTimeOffset? ReturnDate { get; set; }
}