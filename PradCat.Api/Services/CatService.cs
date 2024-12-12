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

    public Task<Response<bool>> DeleteAsync(DeleteCatRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<List<Cat>>> GetAllAsync(GetAllCatsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Cat>> GetByIdAsync(GetCatByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Cat>> UpdateAsync(UpdateCatRequest request)
    {
        throw new NotImplementedException();
    }
}