using PradCat.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests.Appointments;
public class UpdateAppointmentRequest : Request
{
    public int Id { get; set; }

    [Required(ErrorMessage = "DateTime is required.")]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public EAppointmentStatus Status { get; set; }

    [Required(ErrorMessage = "Cat's ID is required.")]
    public int CatId { get; set; }

    [Required(ErrorMessage = "Veterinarian's ID is required.")]
    public int VeterinarianId { get; set; }
}
