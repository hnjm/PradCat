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

    public TutorService(ITutorRepository tutorRepository)
    {
        _tutorRepository = tutorRepository;
    }

    public async Task<Response<Tutor>> CreateAsync(Tutor tutor)
    {
        try
        {
            var createdTutor = await _tutorRepository.CreateAsync(tutor);

            return createdTutor is not null
                ? Response<Tutor>.SuccessResponse(createdTutor, "Tutor created successfully.", 201)
                : Response<Tutor>.ErrorResponse("Failed to create tutor.");
        }
        catch
        {
            return Response<Tutor>.ErrorResponse("An unexpected error occurred.");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            return false;

        var result = await _tutorRepository.DeleteAsync(id);

        return result;
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
                return Response<Tutor>.ErrorResponse("Id out of range.");

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

    public async Task<Response<Tutor>> UpdateAsync(UpdateTutorRequest request, string loggedUserId)
    {
        try
        {
            if (request.Id <= 0)
                return Response<Tutor>.ErrorResponse("Id out of range.");

            var tutor = await _tutorRepository.GetByIdAsync(request.Id);

            if (tutor is null)
                return Response<Tutor>.ErrorResponse("Tutor not found.", 404);

            // Compara o id de usuario do tutor com o id de usuario que esta logado
            if (!Equals(tutor.AppUserId, loggedUserId))
                return Response<Tutor>.ErrorResponse("Not allowed to update user.", 401);

            tutor.Name = request.Name;
            tutor.Address = request.Address;
            tutor.Cpf = request.Cpf;

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

    public async Task<Tutor?> GetByUserIdAsync(string userId)
        => await _tutorRepository.GetByUserIdAsync(userId);

}
