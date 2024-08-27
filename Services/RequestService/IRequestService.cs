using StudentAPI.DTOs;

namespace StudentAPI.Services.RequestService
{
    public interface IRequestService
    {
        Task<List<RequestDTO>?> GetAllRequests();
        Task<List<RequestDTO>?> GetPendingRequests();
        Task<RequestDTO?> ResponceOnRequest(int id, string responce);
    }
}
