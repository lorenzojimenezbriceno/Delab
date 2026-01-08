using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

public class CorporationConfig : IEntityTypeConfiguration<Corporation>
{
    public void Configure(EntityTypeBuilder<Corporation> builder)
    {
        builder.HasKey(e => e.CorporationId);
        builder.HasIndex(x => new { x.Name, x.NroDocument }).IsUnique();
        builder.Property(e => e.DateStart).HasColumnType("date");
        builder.Property(e => e.DateEnd).HasColumnType("date");

        //Evitar el borrado en cascada
        builder.HasOne(e => e.SoftPlan).WithMany(c => c.Corporations).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Country).WithMany(c => c.Corporations).OnDelete(DeleteBehavior.Restrict);
    }
}