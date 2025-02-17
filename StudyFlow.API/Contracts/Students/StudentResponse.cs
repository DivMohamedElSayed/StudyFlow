namespace StudyFlow.API.Contracts.Students;

public record StudentResponse(
    string FirstName,
    string LastName,
    string UserName,
    string PhoneNumber,
    string GradeLevel,
    string SchoolName,
    IList<string> PreferredSubjects
);
