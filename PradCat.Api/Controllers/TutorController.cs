using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1/tutors")]
[Authorize]
public class TutorController : ControllerBase
{
    private readonly ITutorService _tutorService;

    public TutorController(ITutorService tutorService)
    {
        _tutorService = tutorService;
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
        var response = await _tutorService.UpdateAsync(request, userContext);

        return response.StatusCode switch
        {
            200 => Ok(response),
            401 => Unauthorized(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }
}
