using System.ComponentModel.DataAnnotations;

namespace PradCat.Domain.Requests.Users;
public class CreateUserRequest
{
    /// <summary>
    /// Request usado para a criação do usuário da aplicação e, em seguida, o tutor.
    /// </summary>

    // Propriedades IdentityUser

    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MaxLength(20)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [MaxLength(20)]
    [Compare("Password", ErrorMessage = "Password didn't match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    // Propriedades Tutor

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [MaxLength(100, ErrorMessage = "Address must be a maximum of {0} characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF is required.")]
    [MaxLength(11, ErrorMessage = "CPF must be a maximum of {0} characters.")] // validar posteriormente 11 digitos
    public string Cpf { get; set; } = string.Empty;
}
