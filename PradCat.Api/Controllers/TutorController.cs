using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PradCat.Api.Services;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1/tutors")]
[Authorize]
public class TutorController : ControllerBase
{
    private readonly ITutorService _tutorService;
    private readonly UserService _userService;

    public TutorController(ITutorService tutorService, UserService userService)
    {
        _tutorService = tutorService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]GetAllTutorsRequest request)
    {
        var response = await _tutorService.GetAllAsync(request);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        GetTutorByIdRequest request = new GetTutorByIdRequest { Id = id };
        var response = await _tutorService.GetByIdAsync(request);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTutorRequest request)
    {
        request.Id = id;
        var userContext = HttpContext.User;
        var user = await _userService.GetLoggedUserAsync(userContext);

        if (user is null)
            return StatusCode(403, "Not allowed to edit tutor.");
        

        var response = await _tutorService.UpdateAsync(request, user.Id);

        return response.StatusCode switch
        {
            200 => Ok(response),
            401 => Unauthorized(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }
}
