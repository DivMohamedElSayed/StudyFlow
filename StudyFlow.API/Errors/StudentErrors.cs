namespace StudyFlow.API.Errors;

public static class StudentErrors
{
    public static readonly Error DuplicatedStudent =
    new("student.DUPLICATED_STUDENT", "A student with the same details already exists.", StatusCodes.Status409Conflict);
}
