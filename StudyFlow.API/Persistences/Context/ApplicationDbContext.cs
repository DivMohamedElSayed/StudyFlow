namespace StudyFlow.API.Persistences.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser,ApplicationRole,string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Apply all entity configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Identify all foreign keys that have Cascade delete behavior but are not ownership relationships
        var cascadeFKs = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

        // Modify these foreign keys to use Restrict delete behavior instead of Cascade
        foreach (var FK in cascadeFKs)
            FK.DeleteBehavior = DeleteBehavior.Restrict;

        // Call the base OnModelCreating method to finalize the model configuration
        base.OnModelCreating(builder);
    }
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();
}