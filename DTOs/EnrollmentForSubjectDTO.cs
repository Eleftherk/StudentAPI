namespace StudentAPI.DTOs
{
    public record struct EnrollmentForSubjectDTO(String StudFirstName,String StudLastName ,int StudId,int Id, decimal Grade, bool Passed);
}
