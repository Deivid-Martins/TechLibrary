using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Application.Domain.Entities;

namespace TechLibrary.Application.Infraestructure.DataAccess;

public class TechLibraryDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\Users\\Deivid\\Desktop\\Coding\\Visual Studio\\CSharp\\TechLibraryDb.db");
    }
}
