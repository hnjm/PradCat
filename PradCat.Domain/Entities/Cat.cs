using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PradCat.Domain.Entities;

[Table(nameof(Cat))]
public class Cat
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR")]
    [MaxLength(50, ErrorMessage = "Name must be a maximum of {0} characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "CHAR")]
    [MaxLength(1, ErrorMessage = "Gender must be a maximum of {0} characters.")]
    public char Gender { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Range(0, 100, ErrorMessage = "Weight must be a range of {0} to {1}.")]
    public double? Weight { get; set; }

    [Column(TypeName = "NVARCHAR")]
    [MaxLength(30, ErrorMessage = "Breed must be a maximum of {0} characters.")]
    public string? Breed { get; set; } = string.Empty;

    public bool? IsNeutered { get; set; }

    public int TutorId { get; set; }

    [JsonIgnore]
    public Tutor Tutor { get; set; } = null!;

    public List<Appointment> Appointments { get; set; } = new();
}
