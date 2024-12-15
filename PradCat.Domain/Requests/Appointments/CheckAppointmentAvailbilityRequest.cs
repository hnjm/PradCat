using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests.Appointments;
public class CheckAppointmentAvailbilityRequest
{
    [Required(ErrorMessage = "DateTime is required.")]
    [DataType(DataType.DateTime)]
    public DateTime appointmentDate { get; set; }

    [Required(ErrorMessage = "Veterinarian's ID is required.")]
    public int VeterinarianId { get; set; }

}
