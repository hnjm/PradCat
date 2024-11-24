using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PradCat.Domain.Entities;

[Table(nameof(Veterinarian))]
public class Veterinarian
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR")]
    [MaxLength(50, ErrorMessage = "Name must be a maximum of {0} characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "NVARCHAR")]
    [MaxLength(50, ErrorMessage = "Specialty must be a maximum of {0} characters.")]
    public string Specialty { get; set; } = string.Empty;

    public List<Appointment> Appointments { get; set; } = new();
}
