using Microsoft.AspNetCore.Mvc;
using StudentAPI.DTOs;
using StudentAPI.Services.RequestService;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : Controller
    {
        private readonly RequestService _requestService;
        public RequestController(IRequestService requestService) {
            _requestService = (RequestService?)requestService;
        }


        [HttpGet("GetAllRequests")]
        public async Task<ActionResult<List<RequestDTO>>?> GetAllRequests()
        {
            var result = await _requestService.GetAllRequests();
            if (result is null)
            {
                return NotFound("Error.");
            }
            return Ok(result);
        }

        [HttpGet("GetPendingRequests")]
        public async Task<ActionResult<List<RequestDTO>>?> GetPendingRequests()
        {
            var result = await _requestService.GetPendingRequests();
            if (result is null)
            {
                return NotFound("Error.");
            }
            return Ok(result);
        }

        [HttpPut("ResponceOnRequest/{id}/{responce}")]
        public async Task<ActionResult<List<RequestDTO>>?> ResponceOnRequest(int id, string responce)
        {
            var result = await _requestService.ResponceOnRequest(id, responce);
            if (result is null)
            {
                return NotFound("There is no Request with such an Id .");
            }
            return Ok(result);
        }

    }
}
