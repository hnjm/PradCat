using PradCat.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests.Appointments;
public class CreateAppointmentRequest
{
    [Required(ErrorMessage = "DateTime is required.")]
    [DataType(DataType.DateTime)]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public EAppointmentStatus Status { get; set; }

    [Required(ErrorMessage = "Cat's ID is required.")]
    public int CatId { get; set; }

    [Required(ErrorMessage = "Veterinarian's ID is required.")]
    public int VeterinarianId { get; set; }
}
