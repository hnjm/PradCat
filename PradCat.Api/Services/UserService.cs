using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using PradCat.Api.Models;
using PradCat.Domain.Entities;
using PradCat.Domain.Requests.Users;
using PradCat.Domain.Responses;
using System.Security.Claims;

namespace PradCat.Api.Services;

public class UserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Response<string>> LoginAsync(LoginUserRequest request)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
                return Response<string>.SuccessResponse(string.Empty, "User logged in successfully.");

            else if (result.IsLockedOut)
                return Response<string>.ErrorResponse("User locked out, try again later.", 403);

            else
                return Response<string>.ErrorResponse("Invalid login attempt.");
        }
        catch
        {
            return Response<string>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<string>> LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Response<string>.SuccessResponse(string.Empty, "User logged out successfully.");
        }
        catch
        {
            return Response<string>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<AppUser>> CreateAsync(AppUser user, string password)
    {
        try
        {
            var exists = await UserExistsAsync(user.UserName!);
            if (exists)
                return Response<AppUser>.ErrorResponse("User already exist.", 409);

            var result = await _userManager.CreateAsync(user, password);

            return result.Succeeded
                ? Response<AppUser>.SuccessResponse(user, "User created successfully.", 201)
                : Response<AppUser>.ErrorResponse("Failed to create user.");
        }
        catch
        {
            return Response<AppUser>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        // busca pelo nome pois usarname e email sao iguais
        try
        {
            var user = await _userManager.FindByNameAsync(request.Email);

            if (user is null)
                return Response<string>.ErrorResponse("User not found.", 404);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return !string.IsNullOrEmpty(token)
                ? Response<string>.SuccessResponse(token)
                : Response<string>.ErrorResponse("Failed to generate password reset token.");
        }
        catch
        {
            return Response<string>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<bool>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            // busca pelo nome pois usarname e email sao iguais
            var user = await _userManager.FindByNameAsync(request.Email);
            if (user is null)
                return Response<bool>.ErrorResponse("User not found.", 404);

            var result = await _userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);

            return result.Succeeded
                ? Response<bool>.SuccessResponse(true, "Reseted password successfully.")
                : Response<bool>.ErrorResponse("Failed to reset password.");
        }
        catch
        {
            return Response<bool>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<bool>> DeleteAsync(DeleteUserRequest request, ClaimsPrincipal userContext)
    {
        try
        {
            var loggedUser = await _userManager.GetUserAsync(userContext);

            // Se o id do usuario logado for diferente do id do request
            if (loggedUser is null || !Equals(loggedUser.Id, request.Id))
                return Response<bool>.ErrorResponse("Not allowed to delete user.", 401);

            var result = await DeleteAsync(request.Id);

            if (!result.Succeeded)
                return Response<bool>.ErrorResponse("Failed to delete user");

            await _signInManager.SignOutAsync();
            return Response<bool>.SuccessResponse(true, "user deleted successfully.");
        }
        catch
        {
            return Response<bool>.ErrorResponse("An unexpected error occurred.");
        }

    }

    public async Task<IdentityResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return IdentityResult.Failed();
        }

        var result = await _userManager.DeleteAsync(user);
        return result;
    }

    public async Task<IdentityResult> UpdateFkAsync(AppUser user, Tutor tutor)
    {
        user.TutorId = tutor.Id;
        var result = await _userManager.UpdateAsync(user);
        return result;
    }

    public async Task<bool> UserExistsAsync(string userName)
        => await _userManager.FindByNameAsync(userName) is not null;

    public async Task<AppUser?> GetLoggedUserAsync(ClaimsPrincipal userContext)
        => await _userManager.GetUserAsync(userContext);
}
