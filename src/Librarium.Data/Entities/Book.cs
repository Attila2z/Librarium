using System.Dynamic;
using System.Formats.Tar;
using System.Reflection;

namespace Librarium.Data.Entities;

public class Book
{
    public int Id {get; set;}

    public required string Title {get; set;}
    public required string Isbn  {get; set;}
    public int PublicationYear {get; set;}
    public List<Loan> Loans {get; set;} = new();

}

