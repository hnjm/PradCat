using PradCat.Domain.Entities;
using PradCat.Domain.Requests.Tutors;
using PradCat.Domain.Responses;

namespace PradCat.Domain.Handlers.Services;
public interface ITutorService
{
    Task<Response<Tutor>> CreateAsync(CreateTutorRequest request);
    Task<Response<Tutor>> UpdateAsync(UpdateTutorRequest request);
    Task<Response<bool>> DeleteAsync(DeleteTutorRequest request);
    Task<Response<Tutor>> GetByIdAsync(GetTutorByIdRequest request);
    Task<PagedResponse<List<Tutor>>> GetAllAsync(GetAllTutorsRequest request);
}
