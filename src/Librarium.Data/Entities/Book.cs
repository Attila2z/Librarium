namespace Librarium.Data.Entities;

public class Book
{
    public int Id { get; set; }

    public required string Title { get; set; }
    public string? Isbn { get; set; }
    public int PublicationYear { get; set; }
    public bool IsRetired { get; set; }

    public List<Loan> Loans { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
}
