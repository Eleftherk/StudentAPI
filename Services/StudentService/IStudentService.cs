using StudentAPI.DTOs;

namespace StudentAPI.Services.StudentService
{
    public interface IStudentService
    {
        Task<List<StudentReturnDTO>> GetAllStudents();
        Task<StudentReturnDTO?> GetSpecificStudent(int id);
        Task<List<StudentReturnDTO>?> GetSpecificStudents(String firstName,String lastName);
        Task<List<StudentReturnDTO>> CreateStudent(StudentCreateDTO request);
        Task<StudentReturnDTO?> UpdateStudent(int id, StudentCreateDTO request);
        Task<List<Student>?> DeleteStudent(int id);
        Task<StudentReturnDTO?> AddSubjectToStudent(int studentId, List<String> subjectNames);
        Task<StudentReturnDTO?> DeleteSubjectsFromStudent(int id,List<String> names);
        Task<StudentReturnDTO?> AddSubjectToStudentById(int studentId, List<int> subjectIds);
        Task<StudentReturnDTO?> DeleteSubjectsFromStudentById(int id, List<int> ids);
        Task<int> TotalECTS(int id);
        Task<List<StudentReturnDTO>> AddSemester();
        Task<String?> certification(int id);
        Task<StudentReturnDTO?> ChangeGradeOnSubject(int StudentId, int SubjectId, decimal Grade);
        Task<decimal?> GetMeanGrade(int id);
        Task<StudentReturnDTO?> UpdateEmail(int id, string email);
        Task<StudentReturnDTO?> UpdateTell(int id, string tell);
        Task<List<RequestDTO>?> NewRequest(int id, string requestDescription);
        Task<List<RequestDTO>?> GetStudentsRequests(int id);
        Task<StudentReturnDTO?> AddBookToStuden(int enrollmentId, int bookId);
    }
}
