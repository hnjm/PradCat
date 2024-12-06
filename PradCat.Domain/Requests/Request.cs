using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests;
public abstract class Request
{
    [Required(ErrorMessage = "User Id is required.")]
    public string UserId { get; set; } = string.Empty;
}
