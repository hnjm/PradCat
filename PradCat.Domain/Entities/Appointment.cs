using PradCat.Domain.Entities.Enums;

namespace PradCat.Domain.Entities;
public class Appointment
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public EAppointmentStatus Status { get; set; }

    public int CatId { get; set; }
    public required Cat Cat { get; set; }

    public int VeterinarianId { get; set; }
    public required Veterinarian Veterinarian { get; set; }
}
