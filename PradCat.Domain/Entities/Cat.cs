namespace PradCat.Domain.Entities;
public class Cat
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public char Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public double? Weight { get; set; }
    public string? Breed { get; set; } = string.Empty;
    public bool? IsNeutered { get; set; }

    public int TutorId { get; set; }
    public required Tutor Tutor { get; set; }

    public List<Appointment> Appointments { get; set; } = new();
}
