
namespace Biding.Infrastructure
{
    using Biding.Domain.TenderDomain;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using Biding.Domain.BidDomain;

    public class TenderConfig
    {
        public class TenderConfiguration : IEntityTypeConfiguration<Tender>
        {
            public void Configure(EntityTypeBuilder<Tender> entity)
            {
                entity.ToTable("Tenders");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.Property(t => t.ReferenceNumber)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(t => t.Description)
                      .HasMaxLength(2000);

                entity.Property(t => t.IssuedBy)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(t => t.IssueDate)
                      .IsRequired();

                entity.Property(t => t.ClosingDate)
                      .IsRequired();

                entity.Property(t => t.Type)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(t => t.Category)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(t => t.BudgetRange)
                      .HasMaxLength(100);

                entity.Property(t => t.EligibilityCriteria)
                      .HasMaxLength(1000);

                entity.Property(t => t.Location)
                      .HasMaxLength(255);


                //Tender --> Docs , bids
          entity.HasMany<TenderDocument>()
          .WithOne()
          .HasForeignKey(td => td.TenderId)
          .OnDelete(DeleteBehavior.Cascade);

         
        entity.HasMany<Bid>()
        .WithOne()
        .HasForeignKey(b => b.TenderId)
        .OnDelete(DeleteBehavior.Cascade);


            }
        }

    }
}
