using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

public class ManagerConfig : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(e => e.ManagerId);
        builder.HasIndex(e => e.UserName).IsUnique();
        builder.HasIndex(x => new { x.FullName, x.Nro_Document }).IsUnique();
        
        // Evitar el borrado en cascada
        builder.HasOne(e => e.Corporation).WithMany(c => c.Managers).OnDelete(DeleteBehavior.Restrict);
    }
}