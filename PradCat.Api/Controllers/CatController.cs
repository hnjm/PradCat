using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PradCat.Api.Services;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Cats;

namespace PradCat.Api.Controllers;

[ApiController]
[Route("v1/cats")]
[Authorize]
public class CatController : ControllerBase
{
    private readonly ICatService _catService;
    private readonly ITutorService _tutorService;
    private readonly UserService _userService;

    public CatController(ICatService catService, ITutorService tutorService, UserService userService) 
    {
        _catService = catService;
        _tutorService = tutorService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatRequest request)
    {
        var user = await _userService.GetLoggedUserAsync(User);
        if (user == null)
            return StatusCode(403, "Not allowed to create cat.");

        var tutor = await _tutorService.GetByUserIdAsync(user.Id);

        if (tutor == null)
            return StatusCode(400, "An unexpected error occurred.");

        var response = await _catService.CreateAsync(request, tutor.Id);

        return response.StatusCode switch
        {
            201 => Created($"/v1/cats/{response.Data!.Id}", response),
            _ => BadRequest(response)
        };
    }
}
