using StudentAPI.DTOs;

namespace StudentAPI.Services.SubjectService
{
    public interface ISubjectService
    {
        Task<List<SubjectReturnDTO>> GetAllSubjects();
        Task<List<SubjectReturnDTO>> GetAllSubjectsWithoutEnrollments();
        Task<SubjectReturnDTO?> GetSpecificSubjectFromId(int id);
        Task<SubjectReturnDTO?> GetSpecificSubjectFromIdWithoutEnrollments(int id);
        Task<SubjectReturnDTO?> GetSpecificSubjectFromName(String Name);
        Task<SubjectReturnDTO?> GetSpecificSubjectFromNameWithoutEnrollments(String Name);
        Task<List<SubjectReturnDTO>> CreateSubject(SubjectCreateDTO request);
        Task<List<SubjectReturnDTO>?> UpdateSubject(int id, SubjectCreateDTO request);
        Task<List<Subject>?> SetECTS(int id, int ECTS);
        Task<List<Subject>?> SetECTSFromName(String Name, int ECTS);
        Task<List<SubjectReturnDTO>?> DeleteSubject(int id);
        Task<List<Subject>?> DeleteSubjectByName(String name);
    }
}
