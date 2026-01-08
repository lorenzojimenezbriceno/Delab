using Daleb.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

public class StateConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasKey(e => e.CountryId);
        
        // No permite nombres iguales de estados con paises distintos
        builder.HasIndex(e => new { e.Name, e.CountryId }).IsUnique();

        // Proteccion de borrado en cascada
        builder.HasOne(e => e.Country).WithMany(e => e.States).OnDelete(DeleteBehavior.Restrict);
    }
}
