﻿using PradCat.Domain.Entities;
using PradCat.Domain.Requests.Cats;
using PradCat.Domain.Responses;
using System.Security.Claims;

namespace PradCat.Domain.Handlers.Services;
public interface ICatService
{
    Task<Response<Cat>> CreateAsync(CreateCatRequest request, int tutorId);
    Task<Response<Cat>> UpdateAsync(UpdateCatRequest request);
    Task<Response<bool>> DeleteAsync(DeleteCatRequest request);
    Task<Response<Cat>> GetByIdAsync(GetCatByIdRequest request);
    Task<PagedResponse<List<Cat>>> GetAllAsync(GetAllCatsRequest request);
}
