using Microsoft.EntityFrameworkCore;
using PradCat.Api.Repositories;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Repositories;
using PradCat.Domain.Handlers.Services;
using PradCat.Domain.Requests.Cats;
using PradCat.Domain.Responses;

namespace PradCat.Api.Services;

public class CatService : ICatService
{
    private readonly ICatRepository _catRepository;

    public CatService(ICatRepository catRepository)
    {
        _catRepository = catRepository;
    }

    public async Task<Response<Cat>> CreateAsync(CreateCatRequest request, int tutorId)
    {
        try
        {
            Cat cat = new Cat
            {
                Name = request.Name,
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                Weight = request.Weight,
                Breed = request.Breed,
                IsNeutered = request.IsNeutered,
                TutorId = tutorId
            };

            var createdCat = await _catRepository.CreateAsync(cat);

            return createdCat is not null
                    ? Response<Cat>.SuccessResponse(createdCat, "Cat created successfully.", 201)
                    : Response<Cat>.ErrorResponse("Failed to create cat.");
        }
        catch
        {
            return Response<Cat>.ErrorResponse("An unexpected error occurred.");

        }
    }

    public async Task<Response<bool>> DeleteAsync(DeleteCatRequest request, string loggedUserId)
    {
        try
        {
            var cat = await _catRepository.GetByIdAsync(request.Id, loggedUserId);

            if (cat == null)
                return Response<bool>.ErrorResponse("Cat not found.", 404);

            var result = await _catRepository.DeleteAsync(request.Id);

            return result
                ? Response<bool>.SuccessResponse(true, "Cat deleted successfully.")
                : Response<bool>.ErrorResponse("Failed to delete cat.");
        }
        catch
        {
            return Response<bool>.ErrorResponse("An unexpected error occurred.");

        }
    }

    public async Task<PagedResponse<List<Cat>>> GetAllAsync(GetAllCatsRequest request, string loggedUserId)
    {
        try
        {
            var query = _catRepository.GetAll(loggedUserId);

            if (query == null)
                return PagedResponse<List<Cat>>.ErrorPagedResponse("No cats found.", 404);

            var cats = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            return PagedResponse<List<Cat>>.SuccessPagedResponse(cats, totalCount, request.PageSize, request.PageNumber);
        }
        catch
        {
            return PagedResponse<List<Cat>>.ErrorPagedResponse("An unexpected error occurred.");

        }
    }

    public async Task<Response<Cat>> GetByIdAsync(GetCatByIdRequest request, string loggedUserId)
    {
        try
        {
            if (request.Id <= 0)
                return Response<Cat>.ErrorResponse("Id out of range.");

            var cat = await _catRepository.GetByIdAsync(request.Id, loggedUserId);

            return cat is not null
                ? Response<Cat>.SuccessResponse(cat)
                : Response<Cat>.ErrorResponse("Cat not found.", 404);
        }
        catch
        {
            return Response<Cat>.ErrorResponse("An unexpected error occurred.");

        }
    }

    public async Task<Response<Cat>> UpdateAsync(UpdateCatRequest request, string loggedUserId)
    {
        try
        {
            if (request.Id <= 0)
                return Response<Cat>.ErrorResponse("Id out of range.");

            var cat = await _catRepository.GetByIdAsync(request.Id, loggedUserId);

            if (cat is null)
                return Response<Cat>.ErrorResponse("Cat not found.", 404);

            cat.Name = request.Name;
            cat.Gender = request.Gender;
            cat.BirthDate = request.BirthDate;
            cat.Weight = request.Weight;
            cat.Breed = request.Breed;
            cat.IsNeutered = request.IsNeutered;

            var updatedCat = await _catRepository.UpdateAsync(cat);

            return updatedCat is not null
                ? Response<Cat>.SuccessResponse(updatedCat, "Cat updated successfully.")
                : Response<Cat>.ErrorResponse("Cat not found.", 404);
        }
        catch
        {
            return Response<Cat>.ErrorResponse("An unexpected error occurred.");

        }
    }
}