using Microsoft.AspNetCore.Identity;
using PradCat.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PradCat.Api.Models;

public class AppUser : IdentityUser
{
    public int TutorId { get; set; }

    public required Tutor Tutor { get; set; }
}
