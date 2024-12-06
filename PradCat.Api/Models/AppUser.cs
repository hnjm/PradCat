using Microsoft.AspNetCore.Identity;
using PradCat.Domain.Entities;

namespace PradCat.Api.Models;

public class AppUser : IdentityUser
{
    public int? TutorId { get; set; }

    public Tutor? Tutor { get; set; } = null!;
}
