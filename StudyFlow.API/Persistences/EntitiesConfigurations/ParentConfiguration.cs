namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.HasKey(pk => pk.Id);
    }
}
