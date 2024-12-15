using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PradCat.Domain.Requests.Cats;
public class UpdateCatRequest
{
    [JsonIgnore]
    public int Id { get; set; }

    [Required(ErrorMessage = "Cat's name is required.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cat's gender is required.")]
    public char Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    [Range(0.00, 99.99, ErrorMessage = "Weight's range must be between {0} and {1}")] //testar
    public double? Weight { get; set; }

    [MaxLength(100)]
    public string? Breed { get; set; }

    public bool? IsNeutered { get; set; }
}
