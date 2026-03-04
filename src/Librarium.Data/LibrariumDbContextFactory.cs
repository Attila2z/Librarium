using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Librarium.Data;

public class LibrariumDbContextFactory : IDesignTimeDbContextFactory<LibrariumDbContext>
{
    public LibrariumDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibrariumDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=librarium;Username=postgres;Password=postgres");
        return new LibrariumDbContext(optionsBuilder.Options);
    }
}
