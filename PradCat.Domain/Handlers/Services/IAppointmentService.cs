using PradCat.Domain.Entities;
using PradCat.Domain.Requests.Appointments;
using PradCat.Domain.Responses;

namespace PradCat.Domain.Handlers.Services;
public interface IAppointmentService
{
    Task<Response<Appointment>> CreateAsync(CreateAppointmentRequest request);
    Task<Response<Appointment>> UpdateAsync(UpdateAppointmentRequest request);
    Task<Response<bool>> DeleteAsync(DeleteAppointmentRequest request);
    Task<Response<Appointment>> GetByIdAsync(GetAppointmentByIdRequest request);
    Task<PagedResponse<List<Appointment>>> GetAllAsync(GetAllAppointmentsRequest request);
    Task<PagedResponse<List<Appointment>>> GetByDateRangeAsync(GetAppointmentsByDateRangeRequest request);
    Task<Response<bool>> CheckAvailabilityAsync(CheckAppointmentAvailbilityRequest request);
    Task<Response<bool>> CancelAsync(CancelAppointmentRequest request);
}
