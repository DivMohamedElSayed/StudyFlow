namespace StudyFlow.API.Contracts.Students;

public record CreateStudentRequest(
    string GradeLevel,
    string SchoolName,
    string ParentPhoneNumber,
    IList<string> PreferredSubjects
);
