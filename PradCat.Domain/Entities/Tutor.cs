using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Entities;

[Table(nameof(Tutor))]
public class Tutor
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR")]
    [MaxLength(50, ErrorMessage = "Name must be a maximum of {0} characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "NVARCHAR")]
    [MaxLength(100, ErrorMessage = "Address must be a maximum of {0} characters.")]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "NVARCHAR")]
    [MaxLength(11, ErrorMessage = "CPF must be a maximum of {0} characters.")]
    public string Cpf { get; set; } = string.Empty;

    public List<Cat> Cats { get; set; } = new();

    public string AppUserId { get; set; } = string.Empty;

}
