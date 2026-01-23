using Delab.Shared.Entities;
using Delab.Shared.EntitiesSoftSec;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Delab.AccessData.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    /*
     * Entities de la base de datos
    */

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<State> States => Set<State>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<SoftPlan> SoftPlans => Set<SoftPlan>();
    public DbSet<Corporation> Corporations => Set<Corporation>();
    public DbSet<Manager> Managers => Set<Manager>();

    /*
     * Manejo de UserRoles por Usuario
     */

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioRole> UsuarioRoles => Set<UsuarioRole>();
    public DbSet<UserRoleDetails> UserRoleDetails => Set<UserRoleDetails>();

    /*
     * Metodo que se ejecuta inicialmente para el modelo de la base de datos
     */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        // Para tomar los valores de ConfigEntities
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}