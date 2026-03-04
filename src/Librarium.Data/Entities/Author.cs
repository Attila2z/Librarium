using System.Dynamic;
using System.Formats.Tar;
using System.Reflection;

namespace Librarium.Data.Entities;

public class Author
{
    public int Id {get; set;}

    public required string FirstName {get; set;}
    public required string LastName  {get; set;}
    public string? Biography {get; set;}
    
    public List<Book> Books { get; set; } = new();

}