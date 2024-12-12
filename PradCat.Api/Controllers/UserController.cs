using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PradCat.Api.Models;
using PradCat.Api.Services;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Users;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1")]
public class UserController : ControllerBase
{
    private readonly ITutorService _tutorService;
    private readonly UserService _userService;

    public UserController(ITutorService tutorService, UserService userService)
    {
        _tutorService = tutorService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        // Cria o usuario de login
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var userResponse = await _userService.CreateAsync(user, request.Password);

        if (userResponse.StatusCode == 409)
            return Conflict(userResponse);

        if (userResponse.StatusCode == 400)
            return BadRequest(userResponse);

        // Cria o tutor
        var tutor = new Tutor
        {
            Name = request.Name,
            Address = request.Address,
            Cpf = request.Cpf,
            AppUserId = userResponse.Data!.Id
        };

        var tutorResponse = await _tutorService.CreateAsync(tutor);

        if (tutorResponse.StatusCode == 400)
        {
            await _userService.DeleteAsync(userResponse.Data!.Id);
            return BadRequest(tutorResponse);
        }

        // Atualiza a fk do usuario com o id do tutor
        var bindUserWithTutorResult = await _userService.UpdateFkAsync(userResponse.Data!, tutorResponse.Data!);

        if (bindUserWithTutorResult.Succeeded)
            return Created($"/v1/tutors/{tutorResponse.Data!.Id}", tutorResponse);
        else
        {
            await _tutorService.DeleteAsync(tutorResponse.Data!.Id);
            await _userService.DeleteAsync(userResponse.Data.Id);

            return BadRequest(tutorResponse);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var response = await _userService.LoginAsync(request);

        return response.StatusCode switch
        {
            200 => Ok(response),
            403 => StatusCode(403, response),
            _ => BadRequest(response)
        };
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var response = await _userService.LogoutAsync();

        return response.StatusCode == 200
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpPost("users/forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await _userService.ForgotPasswordAsync(request);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }

    [HttpPost("users/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await _userService.ResetPasswordAsync(request);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }

    [Authorize]
    [HttpDelete("users/delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        DeleteUserRequest request = new DeleteUserRequest { Id = id };
        var userContext = HttpContext.User;
        var response = await _userService.DeleteAsync(request, userContext);

        return response.StatusCode switch
        {
            200 => Ok(response),
            401 => Unauthorized(response),
            _ => BadRequest(response)
        };
    }
}
