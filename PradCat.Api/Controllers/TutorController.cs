using Microsoft.AspNetCore.Mvc;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1/tutors")]
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

        if (response.StatusCode == 200)
            return Ok(response);
        else if (response.StatusCode == 404)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        GetTutorByIdRequest request = new GetTutorByIdRequest { Id = id };
        var response = await _tutorService.GetByIdAsync(request);

        if (response.StatusCode == 200)
            return Ok(response);
        else if (response.StatusCode == 404)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTutorRequest request)
    {
        request.Id = id;
        var response = await _tutorService.UpdateAsync(request);

        if (response.StatusCode == 200)
            return Ok(response);
        else if (response.StatusCode == 404)
            return NotFound(response);
        else
            return BadRequest(response);
    }

    // provavelmente deletar esse metodo e deixar linkado ao user controller
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, [FromBody] DeleteTutorRequest request)
    {
        request.Id = id;
        var response = await _tutorService.DeleteAsync(request);

        if (response.StatusCode == 200)
            return Ok(response);
        else if (response.StatusCode == 404)
            return NotFound(response);
        else
            return BadRequest(response);
    }
}
