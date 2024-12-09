using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using PradCat.Api.Models;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Users;
using PradCat.Domain.Responses;
using System.Security.Claims;

namespace PradCat.Api.Services;

public class UserService
{
    private readonly ITutorService _tutorService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public UserService(ITutorService tutorService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _tutorService = tutorService;
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

    public async Task<Response<Tutor>> CreateAsync(CreateUserRequest request)
    {
        try
        {
            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var tutor = new Tutor
            {
                Name = request.Name,
                Address = request.Address,
                Cpf = request.Cpf,
                AppUserId = user.Id
            };

            //Cria o usuario para login
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Response<Tutor>.ErrorResponse("Failed to create user.");

            // Cria o tutor com fk do usuario
            tutor = await _tutorService.CreateAsync(tutor);

            // Se nao criou o tutor, deleta o usuario
            if (tutor is null || tutor.Id <= 0)
            {
                await DeleteAsync(user.Id);
                return Response<Tutor>.ErrorResponse("Failed to create user.");
            }

            // Se criou o tutor, atualiza a fk do tutor na tabela do usuario
            result = await UpdateFkAsync(user.Id, tutor.Id);

            // Se nao vinculou usuario com tutor, deleta ambos
            if (!result.Succeeded)
            {
                await _tutorService.DeleteAsync(tutor.Id);
                await DeleteAsync(user.Id);

                return Response<Tutor>.ErrorResponse("Failed to bind user data.");
            }

            return Response<Tutor>.SuccessResponse(tutor, "User created successfully.", 201);
        }
        catch
        {
            return Response<Tutor>.ErrorResponse("An unexpected error occurred.");
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
            Console.WriteLine("logged user: " + loggedUser?.Id);
            Console.WriteLine("request user: " + request.UserId);

            // Se o id do usuario logado for diferente do id do request
            if (loggedUser is null || !Equals(loggedUser.Id, request.UserId))
                return Response<bool>.ErrorResponse("Not allowed to delete user.", 401);

            var result = await DeleteAsync(request.UserId);

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
}
