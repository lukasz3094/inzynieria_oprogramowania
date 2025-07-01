using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class MeetingRepository(AppDbContext db) : IMeetingRepository
{
    private readonly AppDbContext _db = db;

	public async Task<Meeting?> GetByIdAsync(int id)
        => await _db.Meetings
                     .Include(m => m.Organizer)
                     .Include(m => m.Attendees)
                     .ThenInclude(a => a.User)
                     .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<IEnumerable<Meeting>> GetAllAsync()
        => await _db.Meetings
                     .Include(m => m.Organizer)
                     .Include(m => m.Attendees)
                     .ThenInclude(a => a.User)
                     .ToListAsync();

    public async Task<IEnumerable<Meeting>> GetByOrganizerIdAsync(int organizerId)
        => await _db.Meetings
                     .Where(m => m.OrganizerId == organizerId)
                     .Include(m => m.Attendees)
                     .ThenInclude(a => a.User)
                     .ToListAsync();

    public async Task AddAsync(Meeting meeting)
        => await _db.Meetings.AddAsync(meeting);

    public async Task DeleteAsync(Meeting meeting)
        => _db.Meetings.Remove(meeting);

    public async Task SaveChangesAsync()
        => await _db.SaveChangesAsync();
}
