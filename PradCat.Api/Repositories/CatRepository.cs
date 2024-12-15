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

        var savedCat = await _context.Cats.AsNoTracking()
                                            .Include(c => c.Tutor)
                                            .FirstOrDefaultAsync(c => c.Id == cat.Id);


        return savedCat!;
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

    public IOrderedQueryable<Cat>? GetAll(string userId)
        => _context.Cats.AsNoTracking()
                        .Include(x => x.Tutor)
                        .Where(x => x.Tutor.AppUserId == userId)
                        .OrderBy(x => x.Name);
 

    public async Task<Cat?> GetByIdAsync(int id, string userId)
        => await _context.Cats.AsNoTracking()
                                    .Include(x => x.Tutor)
                                    .FirstOrDefaultAsync(x => x.Id == id 
                                                        && x.Tutor.AppUserId == userId);


    public async Task<Cat?> UpdateAsync(Cat cat)
    {
        _context.Cats.Update(cat);
        await _context.SaveChangesAsync();

        return cat;
    }
}

