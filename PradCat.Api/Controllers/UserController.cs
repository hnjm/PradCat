using Microsoft.AspNetCore.Mvc;
using PradCat.Api.Services;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1/users")]
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
    public async Task<IActionResult> Register (CreateTutorRequest request)
    {
        var response = await _tutorService.CreateAsync(request);

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
}
