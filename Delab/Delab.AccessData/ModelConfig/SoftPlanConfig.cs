using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

public class SoftPlanConfig : IEntityTypeConfiguration<SoftPlan>
{
    public void Configure(EntityTypeBuilder<SoftPlan> builder)
    {
        builder.HasKey(e => e.SoftPlanId);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(e => e.Price).HasPrecision(18, 2);
    }
}