using PradCat.Domain.Entities;

namespace PradCat.Domain.Handlers.Repositories;
public interface IAppointmentRepository
{
    Task<Appointment> CreateAsync(Appointment appointment);
    Task<Appointment> UpdateAsync(Appointment appointment);
    Task<bool> DeleteAsync(int id);
    Task<Appointment?> GetByIdAsync(int id);
    Task<List<Appointment>> GetAllAsync(int userId);
    Task<List<Appointment>> GetByDateRangeAsync(int userId, DateTime? startDate, DateTime? endDate);
    Task<bool> CheckAvailabilityAsync(DateTime dateTime, int veterinarianId);
    Task<bool> CancelAsync(int id);
}
