namespace Librarium.Data.Entities;

public class Member
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }

    // Navigation: a member can have many loans
    public List<Loan> Loans { get; set; } = new();
}