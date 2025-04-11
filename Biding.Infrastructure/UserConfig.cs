
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Biding_management_System.Models;

namespace Biding.Infrastructure
{
    
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.FullName)
                  .HasMaxLength(80)
                  .IsRequired();

            entity.Property(u => u.Email)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(u => u.PasswordHash)
                  .HasMaxLength(256)
                  .IsRequired();

            entity.Property(u => u.CreatedAt)
                  .IsRequired();


            // User Relationships --> bids,tenders,evaluation (as evaluator)

            entity.HasMany(u => u.CreatedTenders)
                .WithOne() 
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


                 entity.HasMany(u => u.Bids)
                .WithOne() 
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);


                 entity.HasMany<Evaluation>()
                .WithOne()
                .HasForeignKey(e => e.EvaluatorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
  }

