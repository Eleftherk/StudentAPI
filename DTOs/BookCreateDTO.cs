namespace StudentAPI.DTOs
{
    public record struct BookCreateDTO(string Title, String Description,
        string Author, string SubjectName);
}
