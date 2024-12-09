using Microsoft.EntityFrameworkCore;
using PradCat.Api.Data;
using PradCat.Domain.Entities;
using PradCat.Domain.Handlers.Repositories;

namespace PradCat.Api.Repositories;

public class TutorRepository : ITutorRepository
{
    private readonly AppDbContext _context;

    public TutorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tutor> CreateAsync(Tutor tutor)
    {
        await _context.Tutors.AddAsync(tutor);
        await _context.SaveChangesAsync();


        return tutor;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tutor = await _context.Tutors.AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id);

        if (tutor == null)
            return false;

        _context.Tutors.Remove(tutor);
        await _context.SaveChangesAsync();

        return true;
    }

    public IOrderedQueryable<Tutor>? GetAll(int pageNumber, int pageSize)
    {
        var query = _context.Tutors.AsNoTracking()
                                    .Include(x => x.Cats)
                                    .OrderBy(x => x.Name);

        return query;
    }

    public async Task<Tutor?> GetByIdAsync(int id)
    {
        var tutor = await _context.Tutors.AsNoTracking()
                                    .Include(x => x.Cats)
                                    .FirstOrDefaultAsync(x => x.Id == id);

        return tutor;
    }

    public async Task<Tutor?> UpdateAsync(Tutor tutor)
    {
        var updatedTutor = await _context.Tutors.AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Id == tutor.Id
                                                                && x.AppUserId == tutor.AppUserId);
        if (updatedTutor is not null)
        {
            updatedTutor.Name = tutor.Name;
            updatedTutor.Address = tutor.Address;
            updatedTutor.Cpf = tutor.Cpf;

            _context.Tutors.Update(updatedTutor);
            await _context.SaveChangesAsync();
        }

        return updatedTutor;
    }
}
