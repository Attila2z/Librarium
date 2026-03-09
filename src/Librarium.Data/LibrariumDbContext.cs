using Librarium.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Librarium.Data;

public class LibrariumDbContext : DbContext
{
    public LibrariumDbContext(DbContextOptions<LibrariumDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // BOOK
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.Title).HasMaxLength(256).IsRequired();
            entity.Property(b => b.Isbn).HasMaxLength(20);
            entity.Property(b => b.IsRetired).IsRequired().HasDefaultValue(false);

            entity.HasIndex(b => b.Isbn).IsUnique();
        });

        // MEMBER
        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(m => m.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(m => m.LastName).HasMaxLength(100).IsRequired();
            entity.Property(m => m.Email).HasMaxLength(320).IsRequired();
            entity.Property(m => m.PhoneNumber).HasMaxLength(30);
            entity.HasIndex(m => m.Email).IsUnique();
        });

        // LOAN
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(l => l.LoanDate).IsRequired();
            entity.Property(l => l.ReturnDate).IsRequired(false);
            entity.Property(l => l.Status).IsRequired().HasDefaultValue(LoanStatus.Active);

            entity.HasIndex(l => l.MemberId); // supports GET /api/loans/{memberId}
            entity.HasIndex(l => l.BookId);

            entity.HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        // AUTHOR
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");
            entity.Property(a => a.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(a => a.LastName).HasMaxLength(100).IsRequired();
            entity.Property(a => a.Biography);

        });
    }
}