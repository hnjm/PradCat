using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PradCat.Domain.Requests.Tutors;
public class UpdateTutorRequest : Request
{
    [JsonIgnore]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [MaxLength(100, ErrorMessage = "Address must be a maximum of {0} characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF is required.")]
    [MaxLength(11, ErrorMessage = "CPF must be a maximum of {0} characters.")]
    public string Cpf { get; set; } = string.Empty;
}
