using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.DTOs;

namespace StudentAPI.Services.RequestService
{
    public class RequestService: IRequestService
    {

        private readonly DataContext _context;
        public RequestService(DataContext context)
        {
            _context = context;
        }




        public async Task<List<RequestDTO>?> GetAllRequests()
        {
            var requests = await _context.Requests.Include(x => x.Student).ToListAsync();
            List<RequestDTO> result = new List<RequestDTO>();
            for (int i = 0; i < requests.Count; i++)
            {
                result.Add(new RequestDTO()
                {
                    Id = requests[i].Id,
                    Description = requests[i].Description,
                    Status = requests[i].Status,
                    Return = requests[i].Return,
                    DateoOfRequest = requests[i].DateOfRequest,
                    DateOfTermination = requests[i].DateOfterminations,
                    StudentId = requests[i].Student.Id,
                    FirstName = requests[i].Student.FirstName,
                    LastName = requests[i].Student.LastName
                });
            }
            return result;
        }

            public async Task<List<RequestDTO>?> GetPendingRequests()
        {
            var requests = await _context.Requests.Include(x => x.Student).Where(x => x.Status == false).ToListAsync();
            List<RequestDTO> result = new List<RequestDTO>();
            for (int i = 0; i < requests.Count; i++)
            {
                result.Add(new RequestDTO()
                {
                    Id = requests[i].Id,
                    Description = requests[i].Description,
                    Status = requests[i].Status,
                    Return = requests[i].Return,
                    DateoOfRequest = requests[i].DateOfRequest,
                    DateOfTermination = requests[i].DateOfterminations,
                    StudentId = requests[i].Student.Id,
                    FirstName = requests[i].Student.FirstName,
                    LastName = requests[i].Student.LastName
                });
            }
            return result;
        }

        public async Task<RequestDTO?> ResponceOnRequest(int id, string responce)
        {
            var request = await _context.Requests.Include(x => x.Student).FirstOrDefaultAsync(x => x.Id == id);
            if (request == null)
            {
                return null;
            }
            request.Return = responce;
            request.DateOfterminations = DateTime.Now;
            request.Status = true;
            await _context.SaveChangesAsync();
            request = await _context.Requests.Include(x => x.Student).FirstOrDefaultAsync(x => x.Id == id);

            RequestDTO result = new RequestDTO()
            {
                Id = request.Id,
                Description = request.Description,
                Status = request.Status,
                Return = request.Return,
                DateoOfRequest = request.DateOfRequest,
                DateOfTermination = request.DateOfterminations,
                StudentId = request.Student.Id,
                FirstName = request.Student.FirstName,
                LastName = request.Student.LastName
            };


            return result;
        }
    }
}
