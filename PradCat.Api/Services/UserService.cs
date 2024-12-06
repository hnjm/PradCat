using Microsoft.AspNetCore.Identity;
using PradCat.Api.Models;
using PradCat.Domain.Results;

namespace PradCat.Api.Services;

public class UserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResult> CreateAsync(string email,
                                                        string phoneNumber,
                                                        string password)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            PhoneNumber = phoneNumber
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
            return new CreateUserResult 
            {
                Succeeded = true, 
                UserId = user.Id 
            };
        else
            return new CreateUserResult 
            { 
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description) 
            };
    }

    public async Task<IdentityResult> UpdateFkAsync(string id, int tutorId)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Failed to find user." }); ;
        }

        user.TutorId = tutorId;
        var result = await _userManager.UpdateAsync(user);
        return result;
    }

    public async Task<IdentityResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Failed to find user." }); ;
        }

        var result = await _userManager.DeleteAsync(user);
        return result;
    }
}
