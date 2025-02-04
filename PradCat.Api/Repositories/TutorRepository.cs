﻿using Microsoft.EntityFrameworkCore;
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

    public IOrderedQueryable<Tutor>? GetAll()
        => _context.Tutors.AsNoTracking()
                                    .Include(x => x.Cats)
                                    .OrderBy(x => x.Name);


    public async Task<Tutor?> GetByIdAsync(int id)
        => await _context.Tutors.AsNoTracking()
                                    .Include(x => x.Cats)
                                    .FirstOrDefaultAsync(x => x.Id == id);


    public async Task<Tutor?> GetByUserIdAsync(string userId)
        => await _context.Tutors.AsNoTracking()
                                .Include(x => x.Cats)
                                .FirstOrDefaultAsync(x => x.AppUserId == userId);


    public async Task<Tutor?> UpdateAsync(Tutor tutor)
    {
        _context.Tutors.Update(tutor);
        await _context.SaveChangesAsync();

        return tutor;
    }
}
