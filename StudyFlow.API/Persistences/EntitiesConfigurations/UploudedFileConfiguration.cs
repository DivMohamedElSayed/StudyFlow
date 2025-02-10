namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class UploudedFileConfiguration : IEntityTypeConfiguration<UploudedFile>
{
    public void Configure(EntityTypeBuilder<UploudedFile> builder)
    {
        builder.Property(f => f.FileName).HasMaxLength(250);
        builder.Property(s => s.StoredFileName).HasMaxLength(250);
        builder.Property(c => c.ContentType).HasMaxLength(50);
        builder.Property(f => f.FileExtention).HasMaxLength(10);
    }
}
