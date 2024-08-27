namespace StudentAPI.DTOs
{
    public record struct SubjectReturnDTO(int Id, String Name, List<BookDTO> Books, int ECTS,List<EnrollmentForSubjectDTO> Enrollments);
   
}
