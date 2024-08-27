namespace StudentAPI.DTOs
{
    public record struct RequestDTO(int Id, string Description,
         bool Status, string Return, DateTime DateoOfRequest, 
         DateTime? DateOfTermination, int StudentId,
         string FirstName, string LastName);
}
