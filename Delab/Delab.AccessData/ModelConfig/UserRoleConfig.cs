using Delab.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delab.AccessData.ModelConfig;

public class UserRoleConfig : IEntityTypeConfiguration<UserRoleDetails>
{
    public void Configure(EntityTypeBuilder<UserRoleDetails> builder)
    {
        builder.HasIndex(e => e.UserRoleDetailsId);
        builder.HasIndex(e => new { e.UserType, e.UserId }).IsUnique();
        //Proteccion de Borrado en Cascada
        builder.HasOne(e => e.User).WithMany(e => e.UserRoleDetails).OnDelete(DeleteBehavior.Restrict);
    }
}