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
        builder.HasData(new ApplicationUser
        {
            Id = DefaultUser.AdminId,
            FirstName = DefaultUser.AdminFirstName,
            LastName = DefaultUser.AdminLastName,
            Email = DefaultUser.AdminEmail,
            NormalizedEmail= DefaultUser.AdminEmail.ToUpper(),
            UserName = DefaultUser.AdminUserName,
            NormalizedUserName = DefaultUser.AdminUserName.ToUpper(),
            IsDisabled =false,
            EmailConfirmed =true,
            ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp,
            SecurityStamp = DefaultUser.AdminSecurityStamp,
            PasswordHash = DefaultUser.AdminPassword,
            ThemePreference =ThemeConstants.Default
        });
        builder.OwnsMany(r => r.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        builder.HasOne(r => r.Student)
           .WithOne(r => r.ApplicationUser)
           .HasForeignKey<Student>(fk => fk.UserId);

        builder.HasOne(r => r.Teacher)
            .WithOne(r => r.ApplicationUser)
            .HasForeignKey<Teacher>(fk => fk.UserId);

        builder.HasOne(r => r.Parent)
            .WithMany(r => r.ApplicationUsers)
            .HasForeignKey(fk => fk.ParentId);
    }
}