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
        if (cat == null)
        {
            throw new ArgumentNullException("Cat is invalid.");
        }

        await _context.Cats.AddAsync(cat);
        await _context.SaveChangesAsync();

        return cat;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException("Invalid Cat ID.");

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
        if (id <= 0)
            throw new ArgumentOutOfRangeException("Invalid Cat ID.");

        var cat = await _context.Cats.AsNoTracking()
                                    .Include(x => x.Tutor)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        return cat;
    }

    public async Task<Cat> UpdateAsync(Cat cat)
    {
        if (cat == null)
            throw new ArgumentNullException("Cat is invalid.");

        var exist = await _context.Cats.AsNoTracking()
                                            .AnyAsync(x => x.Id == cat.Id);
        if (!exist)
            throw new KeyNotFoundException("Cat not found");

        _context.Cats.Update(cat);
        await _context.SaveChangesAsync();

        return cat;
    }
}

