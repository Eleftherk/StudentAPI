namespace StudentAPI.DTOs
{
    public record struct EnrollmentForBookDTO(int enrolmentId,Student student,DateTime? dateOfChoosingBook, Book book);
}
