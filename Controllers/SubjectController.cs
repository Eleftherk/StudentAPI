using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.DTOs;
using StudentAPI.Services.StudentService;
using StudentAPI.Services.SubjectService;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = (SubjectService?)subjectService;
        }

        [HttpPost]
        public async Task<ActionResult<List<Subject>>> CreateStudent(SubjectCreateDTO request)
        {

            var result = await _subjectService.CreateSubject(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SubjectReturnDTO>>> DeleteSubject(int id)
        {
            var result = await _subjectService.DeleteSubject(id);
            if (result is null)
            {
                return NotFound("There is no subject with such an id.");
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<SubjectReturnDTO>>> GetAllSubjects()
        {
            var result = await _subjectService.GetAllSubjects();
            return Ok(result);
        }
        [HttpGet("WithoutEnrollments")]
        public async Task<ActionResult<List<SubjectReturnDTO>>> GetAllSubjectsWithoutEnrollments()
        {
            var result = await _subjectService.GetAllSubjectsWithoutEnrollments();
            return Ok(result);
        }

        [HttpGet("ByNameWithoutEnrollment/{Name}")]
        public async Task<ActionResult<SubjectReturnDTO>> GetSpecificSubjectFromNameWithoutEnrollments(string Name)
        {
            var result = await _subjectService.GetSpecificSubjectFromNameWithoutEnrollments(Name);
            if (result is null)
            {
                return NotFound("There is no subject with such a name.");
            }
            return Ok(result);
        }


        [HttpGet("ByName/{Name}")]
        public async Task<ActionResult<SubjectReturnDTO>> GetSpecificSubjectFromName(string Name)
        {
            var result = await _subjectService.GetSpecificSubjectFromName(Name);
            if (result is null)
            {
                return NotFound("There is no subject with such a name.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectReturnDTO>> GetSpesificSubjectFromId(int id)
        {
            var result = await _subjectService.GetSpecificSubjectFromId(id);
            if (result is null)
            {
                return NotFound("There is no subject with such an Id.");
            }
            return Ok(result);
        }

        [HttpGet("WithoutEnrollments/{id}")]
        public async Task<ActionResult<SubjectReturnDTO>> GetSpesificSubjectFromIdWithoutEnrollments(int id)
        {
            var result = await _subjectService.GetSpecificSubjectFromIdWithoutEnrollments(id);
            if (result is null)
            {
                return NotFound("There is no subject with such an Id.");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<StudentReturnDTO>>> UpdateStudent(int id, SubjectCreateDTO request)
        {
            var result = await _subjectService.UpdateSubject(id, request);
            if (result is null)
            {
                return NotFound("There is no subject with such an Id.");
            }
            return Ok(result);
        }



        [HttpPut("{id}/{ECTS}")]
        public async Task<ActionResult<List<Subject>>> SetECTS(int id, int ECTS)
        {
            var result = await _subjectService.SetECTS(id, ECTS);
            if (result is null)
            {
                return NotFound("There is no subject with such an Id.");
            }
            return Ok(result);

        }
        [HttpPut("ByName/{Name}/{ECTS}")]
        public async Task<ActionResult<List<Subject>>> SetECTSFromName(String Name, int ECTS)
        {
            var result = await _subjectService.SetECTSFromName(Name, ECTS);
            if (result is null)
            {
                return NotFound("There is no subject with such an Name.");
            }
            return Ok(result);
        }


        [HttpDelete("DeleteByName/{Name}")]
        public async Task<ActionResult<List<Subject>>> DeleteSubjectByName(String Name)
        {
            var result = await _subjectService.DeleteSubjectByName(Name);
            if (result is null)
            {
                return NotFound("There is no subject with such an Name.");
            }
            return Ok(result);
        }
        
    }
}


