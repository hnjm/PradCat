using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests.Users;

public class LoginUserRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MaxLength(20)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}
