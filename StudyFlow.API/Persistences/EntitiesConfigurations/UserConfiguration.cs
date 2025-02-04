namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(f => f.FirstName)
            .HasMaxLength(100);
        builder.Property(l => l.LastName)
            .HasMaxLength(100);
        builder.Property(t => t.ThemePreference)
            .HasMaxLength(10)
            .HasDefaultValue(ThemeConstants.Default)
            .HasComment("User's theme preference (dark/light/default)");
    }
}