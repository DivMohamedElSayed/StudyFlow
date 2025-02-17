using Newtonsoft.Json;

namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(s => s.SchoolName)
            .HasMaxLength(100);
        builder.Property(g => g.GradeLevel)
            .HasMaxLength(20);
        builder.Property(p => p.ParentPhoneNumber)
            .HasMaxLength(12);
        builder.Property(p => p.PreferredSubjects)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v) ?? new List<string>()
            );
    }
}
