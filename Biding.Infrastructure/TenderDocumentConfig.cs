

namespace Biding.Infrastructure
{
   
    using Biding.Domain.TenderDomain;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    public class TenderDocumentConfig
    {
        public class TenderDocumentConfiguration : IEntityTypeConfiguration<TenderDocument>
        {
            public void Configure(EntityTypeBuilder<TenderDocument> entity)
            {
                entity.ToTable("TenderDocuments");

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
