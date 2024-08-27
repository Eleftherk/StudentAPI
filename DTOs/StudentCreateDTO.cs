namespace StudentAPI.DTOs
{
    public record struct StudentCreateDTO(string FirstName, string LastName, List<SubjectCreateDTO> Subjects, 
        string Tell = "-", string email = "-", int CurrntSemester = 1);
}
