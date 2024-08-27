namespace StudentAPI.DTOs
{
    namespace StudentAPI.DTOs
    {
        public record struct EnrollmentForStudentDTO(String SubjName, int SubjId, int Id, decimal Grade, bool Passed,BookDTO? book);
    }
}
