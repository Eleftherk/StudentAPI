namespace StudentAPI.DTOs
{
    public record struct BookDTO(int? Id,string? Title, String? Description,
        string? Author, string? SubjectName);
}
