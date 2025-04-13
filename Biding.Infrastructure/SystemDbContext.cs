namespace Biding_management_System.Data;

using Microsoft.EntityFrameworkCore;
using Biding_management_System.Models;
using Biding.Domain.BidDomain;
using Biding.Domain.TenderDomain;
using Microsoft.Extensions.Configuration;
using System.Reflection;

public class SystemDbContext : DbContext
{
    private readonly IConfiguration Configuration;
    public DbSet<User> Users { get; set; }
    public DbSet<Tender> Tenders { get; set; }
    public DbSet<TenderDocument> TenderDocuments { get; set; }
    public DbSet<Bid> Bids { get; set; }
    public DbSet<BidDocument> BidDocuments { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }

    public SystemDbContext(DbContextOptions<SystemDbContext> options)
        : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //My server's connection string.
        optionsBuilder.UseSqlServer("Server=DESKTOP-R6A26BO\\SQLEXPRESS;Database=BidingManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;");

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Search for my configs
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //just for the Role in User to achive RBAC in my system.
        modelBuilder.Entity<User>()
      .Property(u => u.Role)
      .HasConversion(
          v => v.ToString(), 
          v => (UserRole)Enum.Parse(typeof(UserRole), v) 
      )
      .HasMaxLength(50) 
      .IsRequired();

        //just for the Status in Bid to achive logical scoring system.
        modelBuilder.Entity<Bid>()
      .Property(b => b.Status)
      .HasConversion(
          v => v.ToString(),
          v => (Status)Enum.Parse(typeof(Status), v)
      )
      .HasMaxLength(50)
      .IsRequired();
    }
}
