using PradCat.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PradCat.Domain.Entities;

[Table(nameof(Appointment))]
public class Appointment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    [Required]
    public EAppointmentStatus Status { get; set; }


    public int CatId { get; set; }
    public required Cat Cat { get; set; }

    public int VeterinarianId { get; set; }
    public required Veterinarian Veterinarian { get; set; }
}
