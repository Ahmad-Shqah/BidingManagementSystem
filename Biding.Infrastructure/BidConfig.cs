
namespace Biding.Infrastructure
{
    using Biding_management_System.Models;
    using Biding.Domain.BidDomain;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    public class BidConfig
    {
        public class BidConfiguration : IEntityTypeConfiguration<Bid>
        {
            public void Configure(EntityTypeBuilder<Bid> entity)
            {
                entity.ToTable("Bids");

                entity.HasKey(b => b.Id);

                entity.Property(b => b.ProposedAmount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(b => b.TechnicalProposal)
                      .HasMaxLength(2000);

                entity.Property(b => b.FinancialProposal)
                      .HasMaxLength(2000);

                entity.Property(b => b.SubmittedAt)
                      .IsRequired();

                entity.Property(b => b.Status)
                      .IsRequired();
                entity.Property(b => b.Score)
                     .HasColumnType("decimal(18,2)")
                     .IsRequired();
                //Bid Relationships --> Docs ,evaluation

                entity.HasMany<BidDocument>()
                 .WithOne()
                 .HasForeignKey(bd => bd.BidId)
                 .OnDelete(DeleteBehavior.Cascade); ;


                entity.HasMany<Evaluation>()
                .WithOne()
                .HasForeignKey(e => e.BidId)
                .OnDelete(DeleteBehavior.Cascade);


            }
        }
    }
}
