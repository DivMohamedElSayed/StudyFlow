namespace StudyFlow.API.Persistences.EntitiesConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([
            new ApplicationRole
            {
                Id = DefaultRoles.AdminRoleId,
                Name = DefaultRoles.Admin,
                NormalizedName = DefaultRoles.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = DefaultRoles.StudentRoleId,
                Name = DefaultRoles.Student,
                NormalizedName = DefaultRoles.Student.ToUpper(),
                ConcurrencyStamp = DefaultRoles.StudentRoleConcurrencyStamp,
                IsDefault = true
            },
            new ApplicationRole
            {
                Id = DefaultRoles.TeacherRoleId,
                Name = DefaultRoles.Teacher,
                NormalizedName= DefaultRoles.Teacher.ToUpper(),
                ConcurrencyStamp = DefaultRoles.TeacherRoleConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = DefaultRoles.ParentRoleId,
                Name = DefaultRoles.Parent,
                NormalizedName = DefaultRoles.Parent.ToUpper(),
                ConcurrencyStamp = DefaultRoles.ParentRoleConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = DefaultRoles.GuestRoleId,
                Name = DefaultRoles.Guest,
                NormalizedName = DefaultRoles.Guest.ToUpper(),
                ConcurrencyStamp = DefaultRoles.GuestRoleConcurrencyStamp
            }
        ]);
    }
}
