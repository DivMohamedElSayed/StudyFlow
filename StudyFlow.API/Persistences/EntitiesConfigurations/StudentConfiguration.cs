namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(s => s.SchoolName)
            .HasMaxLength(100);
    }
}
