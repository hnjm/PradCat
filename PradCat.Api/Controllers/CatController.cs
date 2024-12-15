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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]GetAllCatsRequest request)
    {
        var loggedUser = await _userService.GetLoggedUserAsync(User);
        if (loggedUser == null)
            return Unauthorized("Login is needed.");

        var response = await _catService.GetAllAsync(request, loggedUser.Id);

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
        var request = new GetCatByIdRequest { Id = id };

        var loggedUser = await _userService.GetLoggedUserAsync(User);
        if (loggedUser == null)
            return Unauthorized("Login is needed.");

        var response = await _catService.GetByIdAsync(request, loggedUser.Id);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatRequest request)
    {
        var loggedUser = await _userService.GetLoggedUserAsync(User);
        if (loggedUser == null)
            return Unauthorized("Login is needed.");

        var tutor = await _tutorService.GetByUserIdAsync(loggedUser.Id);

        if (tutor == null)
            return StatusCode(400, "An unexpected error occurred.");

        var response = await _catService.CreateAsync(request, tutor.Id);

        return response.StatusCode switch
        {
            201 => Created($"/v1/cats/{response.Data!.Id}", response),
            _ => BadRequest(response)
        };
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCatRequest request)
    {
        request.Id = id;

        var loggedUser = await _userService.GetLoggedUserAsync(User);
        if (loggedUser == null)
            return Unauthorized("Login is needed.");

        var response = await _catService.UpdateAsync(request, loggedUser.Id);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var loggedUser = await _userService.GetLoggedUserAsync(User);
        if (loggedUser == null)
            return Unauthorized("Login is needed.");

        var request = new DeleteCatRequest { Id = id };

        var response = await _catService.DeleteAsync(request, loggedUser.Id);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            _ => BadRequest(response)
        };
    }
}
