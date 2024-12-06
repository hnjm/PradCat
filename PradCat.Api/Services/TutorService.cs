using Microsoft.EntityFrameworkCore;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Repositories;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Tutors;
using PradCat.Domain.Responses;

namespace PradCat.Api.Services;

public class TutorService : ITutorService
{
    private readonly ITutorRepository _tutorRepository;
    private readonly UserService _userService;

    public TutorService(ITutorRepository tutorRepository, UserService userService)
    {
        _tutorRepository = tutorRepository;
        _userService = userService;
    }

    public async Task<Response<Tutor>> CreateAsync(CreateTutorRequest request)
    {
        try
        {
            // Cria o usuario para login
            var result = await _userService.CreateAsync(request.Email,
                                                        request.PhoneNumber,
                                                        request.Password);


            if (!result.Succeeded || string.IsNullOrEmpty(result.UserId))
            {
                var errors = string.Join(", ", result.Errors
                    ?? new List<string> { "User creation failed without specific errors." });

                return Response<Tutor>.ErrorResponse(errors);
            }

            var tutor = new Tutor
            {
                Name = request.Name,
                Address = request.Address,
                Cpf = request.Cpf,
                AppUserId = result.UserId
            };

            // Cria o tutor com fk de user
            var createdTutor = await _tutorRepository.CreateAsync(tutor);

            if (createdTutor.Id <= 0)
            {
                await _userService.DeleteAsync(result.UserId);
                return Response<Tutor>.ErrorResponse("Could not create tutor.");
            }

            // se criou o tutor, atualiza user com fk de tutor
            var updateResult = await _userService.UpdateFkAsync(result.UserId, createdTutor.Id);

            if (!updateResult.Succeeded)
            {
                await _userService.DeleteAsync(result.UserId);
                await _tutorRepository.DeleteAsync(createdTutor.Id, result.UserId); // testar se exclui em cascata
                return Response<Tutor>.ErrorResponse("Could not create tutor.");
            }

            return Response<Tutor>.SuccessResponse(createdTutor, "Tutor created successfully.", 201);
        }
        catch
        {
            return Response<Tutor>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<bool>> DeleteAsync(DeleteTutorRequest request)
    {
        try
        {
            if (request.Id <= 0 || string.IsNullOrEmpty(request.UserId))
                return Response<bool>.ErrorResponse("Arguments null or out of range.");

            var result = await _tutorRepository.DeleteAsync(request.Id, request.UserId);

            return result 
                ? Response<bool>.SuccessResponse(result, "Tutor deleted successfully") 
                : Response<bool>.ErrorResponse("Tutor not found", 404);
        }
        catch
        {
            return Response<bool>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<PagedResponse<List<Tutor>>> GetAllAsync(GetAllTutorsRequest request)
    {
        try
        {
            var query = _tutorRepository.GetAll(request.PageNumber, request.PageSize);

            if (query == null)
                return PagedResponse<List<Tutor>>.ErrorPagedResponse("No tutor has been found.", 404);

            var tutors = await query
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();

            var totalCount = await query.CountAsync();

            return PagedResponse<List<Tutor>>.SuccessPagedResponse(tutors, totalCount, request.PageSize, request.PageNumber);

        }
        catch
        {
            return PagedResponse<List<Tutor>>.ErrorPagedResponse("An unexpected error occurred.");
        }
    }

    public async Task<Response<Tutor>> GetByIdAsync(GetTutorByIdRequest request)
    {
        try
        {
            if (request.Id <= 0)
                return Response<Tutor>.ErrorResponse("Arguments null or out of range.");

            var tutor = await _tutorRepository.GetByIdAsync(request.Id);

            return tutor is not null
                ? Response<Tutor>.SuccessResponse(tutor)
                : Response<Tutor>.ErrorResponse("Tutor not found", 404);
        }
        catch
        {
            return Response<Tutor>.ErrorResponse("An unexpected error occurred.");

        }
    }

    public async Task<Response<Tutor>> UpdateAsync(UpdateTutorRequest request)
    {
        try
        {
            if (request.Id <= 0 || string.IsNullOrEmpty(request.UserId))
                return Response<Tutor>.ErrorResponse("Arguments null or out of range.");

            var tutor = new Tutor
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                Cpf = request.Cpf,
                AppUserId = request.UserId
            };

            var updatedTutor = await _tutorRepository.UpdateAsync(tutor);

            return updatedTutor is not null
                ? Response<Tutor>.SuccessResponse(updatedTutor, "Tutor updated successfully")
                : Response<Tutor>.ErrorResponse("Tutor not found", 404);
        }
        catch
        {
            return Response<Tutor>.ErrorResponse("An unexpected error occurred.");

        }
    }
}
