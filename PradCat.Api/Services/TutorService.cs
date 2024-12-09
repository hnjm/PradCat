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

    public async Task<Tutor?> CreateAsync(Tutor tutor)
    {
        if (string.IsNullOrEmpty(tutor.AppUserId))
        {
            return null;
        }

        var createdTutor = await _tutorRepository.CreateAsync(tutor);

        return createdTutor;
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
