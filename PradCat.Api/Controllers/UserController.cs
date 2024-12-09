using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PradCat.Api.Services;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;
using PradCat.Domain.Requests.Users;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ITutorService _tutorService;
    private readonly UserService _userService;

    public UserController(ITutorService tutorService , UserService userService)
    {
        _tutorService = tutorService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        var response = await _userService.CreateAsync(request);

        if (response.StatusCode == 201)
        {
            var uri = $"/v1/tutors/{response.Data!.Id}";
            return Created(uri, response);
        }
        else if (response.StatusCode == 404)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var response = await _userService.Login(request);

        return response.StatusCode == 200 
            ? Ok(response) 
            : BadRequest(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var response = await _userService.Logout();

        return response.StatusCode == 200
            ? Ok(response)
            : BadRequest(response);
    }

    [HttpDelete("users/delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        DeleteUserRequest request = new DeleteUserRequest { UserId = id };
        var userContext = HttpContext.User;
        var response = await _userService.DeleteAsync(request, userContext);

        return response.StatusCode switch
        {
            200 => Ok(response),
            401 => Unauthorized(response),
            _ => BadRequest(response),
        };
    }
}
