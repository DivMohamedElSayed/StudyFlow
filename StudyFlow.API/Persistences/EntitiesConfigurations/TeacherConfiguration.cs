namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(s => s.Subject)
            .HasMaxLength(100);

    }
}
