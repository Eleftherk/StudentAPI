using StudentAPI.DTOs.StudentAPI.DTOs;

namespace StudentAPI.DTOs
{
    public record struct StudentReturnDTO(int ID, string FirstName, string LastName, int CurrentSemester,
        string Tell, string Email, DateTime DateOfRegistration, List<EnrollmentForStudentDTO> Enrollments);

}
