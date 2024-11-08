namespace PradCat.Domain.Entities;
public class Veterinarian
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public List<Appointment> Appointments { get; set; } = new();
}
