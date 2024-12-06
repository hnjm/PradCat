using PradCat.Domain.Entities;

namespace PradCat.Domain.Handlers.Repositories;
public interface ITutorRepository
{
    Task<Tutor> CreateAsync(Tutor tutor);
    Task<Tutor?> UpdateAsync(Tutor tutor);
    Task<bool> DeleteAsync(int id, string userId);
    Task<Tutor?> GetByIdAsync(int id);
    IOrderedQueryable<Tutor>? GetAll(int pageNumber, int pageSize);
}
