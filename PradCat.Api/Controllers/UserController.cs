using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PradCat.Api.Services;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;
using PradCat.Domain.Requests.Users;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1")]
public class UserController : ControllerBase
{
    private readonly ITutorService _tutorService;
    private readonly UserService _userService;

    public UserController(ITutorService tutorService , UserService userService)
    {
        _tutorService = tutorService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        var response = await _userService.CreateAsync(request);

        return response.StatusCode switch
        {
            201 => Created($"/v1/tutors/{response.Data!.Id}", response),
            409 => Conflict(response),
            _ => BadRequest(response)
        };
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
