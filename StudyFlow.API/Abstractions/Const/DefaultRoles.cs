namespace StudyFlow.API.Abstractions.Const;

public static class DefaultRoles
{
    public const string Admin = nameof(Admin);
    public const string AdminRoleId = "1AC74F3B-4C92-42B7-9E6E-AD7E75E5771C";
    public const string AdminRoleConcurrencyStamp = "15F497FA-8AC2-4F62-987C-1BB31EA5F9A3";

    public const string Student = nameof(Student);
    public const string StudentRoleId = "E99790C2-ED3B-4C39-9C74-9C7513547029";
    public const string StudentRoleConcurrencyStamp = "ADDFBE9A-C49B-48E2-8F75-900DB06F5777";

    public const string Teacher = nameof(Teacher);
    public const string TeacherRoleId = "D2A3C1E5-8B22-4C78-9F44-3B8B5F4A7E6D";
    public const string TeacherRoleConcurrencyStamp = "5F9A3D7B-6C12-4F88-BE47-1A2F9C8D6712";

    public const string Parent = nameof(Parent);
    public const string ParentRoleId = "8F12A3D5-6C44-4F99-AE35-2D9B4C5E8A71";
    public const string ParentRoleConcurrencyStamp = "A71C4D8F-3B29-4F61-82E7-9D4A6C8F371B";

    public const string Guest = nameof(Guest);
    public const string GuestRoleId = "3C5D8A1F-7B44-4E99-AC61-5F7D3B29E4A8";
    public const string GuestRoleConcurrencyStamp = "C8A1F3D5-6B29-4F77-91E2-4D9B5C6A731F";
}
