

namespace Biding.Infrastructure
{
    using Biding_management_System.Models;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;

    public class evaluationConfig
    {
        public void Configure(EntityTypeBuilder<Evaluation> entity)
        {
            entity.ToTable("Evaluations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Score)
                  .IsRequired();

            entity.Property(e => e.Comment)
                  .HasMaxLength(1000);

            entity.Property(e => e.EvaluationDate)
                  .IsRequired();

          
        }
    }
}

