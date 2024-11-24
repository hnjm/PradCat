using PradCat.Domain.Entities.Enums;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Repositories;
using PradCat.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace PradCat.Api.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CancelAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException("Invalid Appointment ID.");

        var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);

        if (appointment == null)
            return false;

        appointment.Status = EAppointmentStatus.Canceled;

        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CheckAvailabilityAsync(DateTime dateTime, int veterinarianId)
    {
        if (dateTime.Ticks < DateTime.Now.Ticks || veterinarianId <= 0)
            throw new ArgumentOutOfRangeException("DateTime or Veterinarian Id is invalid.");

        // Checks if there is any appointment for datetime and vet provided
        var available = await _context.Appointments.AsNoTracking()
                                                .AnyAsync(x => x.VeterinarianId == veterinarianId
                                                            && x.DateTime.Equals(dateTime));

        // returns false if there is any appointment
        return !available;
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        if (appointment is null)
            throw new ArgumentNullException("Appointment is invalid.");

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        return appointment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException("Invalid Appointment ID.");

        var appointment = await _context.Appointments.AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id);

        if (appointment == null)
            return false;

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Appointment>> GetAllAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");


        var appointments = await _context.Appointments.AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .ThenInclude(x => x.Tutor)
                                                    .Where(x => x.Cat.TutorId == userId)
                                                    .ToListAsync();

        return appointments;
    }

    public async Task<List<Appointment>> GetByDateRangeAsync(int userId, DateTime? startDate, DateTime? endDate)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        var appointments = await _context.Appointments.AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .ThenInclude(x => x.Tutor)
                                                    .Where(x => x.Cat.TutorId == userId
                                                        && (startDate == null || x.DateTime >= startDate)
                                                        && (endDate == null || x.DateTime <= endDate))
                                                    .ToListAsync();

        return appointments;
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException("Invalid Appointment ID.");

        var appointment = await _context.Appointments.AsNoTracking()
                                                    .Include(x => x.Cat)
                                                    .ThenInclude(x => x.Tutor)
                                                    .FirstOrDefaultAsync(x => x.Id == id);

        return appointment;
    }

    public async Task<Appointment> UpdateAsync(Appointment appointment)
    {
        if (appointment == null)
            throw new ArgumentNullException("Appointment is invalid.");

        var exist = await _context.Appointments.AsNoTracking()
                                                .AnyAsync(x => x.Id == appointment.Id);
        if (!exist)
            throw new KeyNotFoundException("Appointment not found");

        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();

        return appointment;
    }
}

