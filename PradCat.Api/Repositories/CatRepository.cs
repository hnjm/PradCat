using Microsoft.EntityFrameworkCore;
using PradCat.Api.Data;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Repositories;

namespace PradCat.Api.Repositories;

public class CatRepository : ICatRepository
{
    private readonly AppDbContext _context;

    public CatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cat> CreateAsync(Cat cat)
    {
        await _context.Cats.AddAsync(cat);
        await _context.SaveChangesAsync();

        return cat;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cat = await _context.Cats.AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id);

        if (cat == null)
            return false;

        _context.Cats.Remove(cat);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Cat>> GetAllAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("Invalid user ID.");

        var cats = await _context.Cats.AsNoTracking()
                                    .Include(x => x.Tutor)
                                    .Where(x => x.TutorId == userId)
                                    .ToListAsync();

        return cats;
    }

    public async Task<Cat?> GetByIdAsync(int id)
    {
        var cat = await _context.Cats.AsNoTracking()
                                    .Include(x => x.Tutor)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        return cat;
    }

    public async Task<Cat?> UpdateAsync(Cat cat)
    {
        var updatedCat = await _context.Cats.AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Id == cat.Id
                                                                && x.TutorId == cat.TutorId);
        if (updatedCat is not null)
        {
            updatedCat.Name = cat.Name;
            updatedCat.Gender = cat.Gender;
            updatedCat.BirthDate = cat.BirthDate;
            updatedCat.Weight = cat.Weight;
            updatedCat.Breed = cat.Breed;
            updatedCat.IsNeutered = cat.IsNeutered;

            _context.Cats.Update(updatedCat);
            await _context.SaveChangesAsync();
        }

        return updatedCat;
    }
}

