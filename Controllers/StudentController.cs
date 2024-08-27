using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.DTOs;
using StudentAPI.Services.StudentService;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService2 _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = (StudentService2?)studentService;
        }


        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<List<StudentReturnDTO>>> GetAllStudents(){
            var result = await _studentService.GetAllStudents();
            return Ok(result);
        }

        [HttpGet("GetSpesificStudent/{id}")]
        public async Task<ActionResult<StudentReturnDTO>> GetSpesificStudent(int id)
        {
            var result = await _studentService.GetSpecificStudent(id);
            if (result is null)
            {
                return NotFound("There is no student with such an Id.");
            }
            return Ok(result);
        }

        [HttpGet("GetSpesificStudent/{firstName}/{lastName}")]
        public async Task<ActionResult<List<StudentReturnDTO>>> GetSpesificStudent(String firstName,String lastName)
        {
            var result = await _studentService.GetSpecificStudents(firstName,lastName);
            if (result is null)
            {
                return NotFound("There is no student with such a name.");
            }
            return Ok(result);
        }

        [HttpPost("CreateStudent")]
        public async Task<ActionResult<List<StudentReturnDTO>>> CreateStudent(StudentCreateDTO request)
        {
            var result = await _studentService.CreateStudent(request);
            return Ok(result);

        }

        [HttpPut("UpdateStudent/{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, StudentCreateDTO request)
        {
            var result = await _studentService.UpdateStudent(id, request);
            if (result is null)
            {
                return NotFound("There is no student with such an Id.");
            }
            return Ok(result);
        }

        [HttpDelete("DeleteStudent/{id}")]
        public async Task<ActionResult<List<Student>>> DeleteStudent(int id)
        {
            var result = await _studentService.DeleteStudent(id);
            if (result is null)
            {
                return NotFound("There is no student with such an id.");
            }
            return Ok(result);
        }

        [HttpPut("AddSubject/{studentId}")]
        public async Task<ActionResult<StudentReturnDTO>> AddSubjectToStudent(int studentId, List<String> subjectNames)
        {
            var result = await _studentService.AddSubjectToStudent(studentId, subjectNames);
            if (result is null)
            {
                return NotFound("There isn't this id or some subjects don't exists.Try again.");
            }
            return Ok(result);

        }
        [HttpPut("DeleteSubject/{id}")]
        public async Task<ActionResult<StudentReturnDTO>> DeleteSubjectsFromStudent(int id, List<String> names)
        {
            var result = await _studentService.DeleteSubjectsFromStudent(id, names);
            if (result is null)
            {
                return NotFound("Invalid id or null subject names.");
            }
            return Ok(result);
        }

        [HttpPut("AddSubjectById/{studentId}")]
        public async Task<ActionResult<StudentReturnDTO>> AddSubjectToStudentById(int studentId, List<int> subjectIds)
        {
            var result = await _studentService.AddSubjectToStudentById(studentId, subjectIds);
            if (result is null)
            {
                return NotFound("There isn't this id or some subjects don't exists.Try again.");
            }
            return Ok(result);
        }
        [HttpPut("DeleteSubjectById/{id}")]
        public async Task<ActionResult<StudentReturnDTO>> DeleteSubjectsFromStudentById(int id, List<int> ids)
        {
            var result = await _studentService.DeleteSubjectsFromStudentById(id, ids);
            if (result is null)
            {
                return NotFound("Invalid id or null subject names.");
            }
            return Ok(result);
        }
        [HttpGet("ECTS/{id}")]
        public async Task<ActionResult<int>> TotalECTS(int id)
        {
            var result = await _studentService.TotalECTS(id);
            if (result is 0)
            {
                return NotFound("Invalid id or there arent any subjects or subjects dont have ECTS.");
            }
            return Ok(result);
        }

        [HttpPut("AddSemester")]
        public async Task<ActionResult<Student>> AddSemester()
        {
            var result = await _studentService.AddSemester();
            if (result is null)
            {
                return NotFound("There is no student with such an Id.");
            }
            return Ok(result);
        }
        [HttpGet("Certification/{id}")]
        public async Task<ActionResult<String>> GetSertification(int id)
        {
            var result = await _studentService.certification(id);
            if (result is null)
            {
                return NotFound("There is no student with such an Id.");
            }
            return Ok(result);
        }

        [HttpPut("AddGrade/{StudentId}/{SubjectId}/{Grade}")]
        public async Task<ActionResult<Student>> ChangeGradeOnSubject(int StudentId, int SubjectId, decimal Grade)
        {
            var result = await _studentService.ChangeGradeOnSubject(StudentId, SubjectId, Grade);
            if (result is null)
            {
                return NotFound("There is no student with such an Id or subject with such a name or the grade is not allowed.");
            }
            return Ok(result);
        }

        [HttpGet("MeanGrade/{id}")]
        public async Task<ActionResult<String>> GetMeanGrade(int id)
        {
            var result = await _studentService.GetMeanGrade(id);
            if (result is null)
            {
                return NotFound("There is no student with such an Id.");
            }
            return Ok(result);
        }

        [HttpPut("UpdateEmail/{id}/{email}")]
        public async Task<ActionResult<Student>> UpdateEmail(int id, string email)
        {
            var result = await _studentService.UpdateEmail(id, email);
            if (result is null)
            {
                return NotFound("There is no student with such an Id .");
            }
            return Ok(result);
        }


        [HttpPut("UpdateTell/{id}/{tell}")]
        public async Task<ActionResult<Student>> UpdateTell(int id, string tell)
        {
            var result = await _studentService.UpdateTell(id, tell);
            if (result is null)
            {
                return NotFound("There is no student with such an Id .");
            }
            return Ok(result);
        }

        [HttpPut("NewRequest/{id}")]
        public async Task<ActionResult<List<RequestDTO>>?> NewRequest (int id, [FromBody] string requestDescription)
        {
            var result = await _studentService.NewRequest(id, requestDescription);
            if (result is null)
            {
                return NotFound("There is no student with such an Id .");
            }
            return Ok(result);
        }
        [HttpGet("GetStudentsRequests/{id}")]
        public async Task<ActionResult<List<RequestDTO>>?> GetStudentsRequests(int id)
        {
            var result = await _studentService.GetStudentsRequests(id);
            if (result is null)
            {
                return NotFound("There is no student with such an Id .");
            }
            return Ok(result);
        }

        [HttpPut("AddBookToStudent/{enrollmentId}/{bookId}")]
        public async Task<ActionResult<List<StudentReturnDTO>>?> AddBookToStudent(int enrollmentId, int bookId)
        {
            var result = await _studentService.AddBookToStuden(enrollmentId, bookId);
            if (result is null)
            {
                return NotFound("There is no enrolment or book with such an Id or there is already a book or not matching book-subject.");
            }
            return Ok(result);
        }
    }
}

