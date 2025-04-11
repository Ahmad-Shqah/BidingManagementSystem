

namespace Biding.Infrastructure
{
    using Biding.Domain.BidDomain;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    public class BidDocumentConfig
    {
        public class BidDocumentConfiguration : IEntityTypeConfiguration<BidDocument>
        {
            public void Configure(EntityTypeBuilder<BidDocument> entity)
            {
                entity.ToTable("BidDocuments");

                entity.HasKey(d => d.Id);

                entity.Property(d => d.FileName)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(d => d.FilePath)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(d => d.UploadedAt)
                      .IsRequired();
            }
        }

    }
}
