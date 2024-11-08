using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests.Appointments;
public class GetAppointmentsByDateRangeRequest : PagedRequest
{
    [DataType(DataType.Date)]
    public DateTime? startDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? endDate { get; set; }
}

