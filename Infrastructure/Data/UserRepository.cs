using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class UserRepository(AppDbContext db) : IUserRepository
{
    private readonly AppDbContext _db = db;

	public async Task<User?> GetByIdAsync(int id)
        => await _db.Users
					.Include(u => u.OrganizedMeetings)
					.Include(u => u.MeetingsAttended)
					.ThenInclude(a => a.Meeting)
					.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<User>> GetAllAsync()
        => await _db.Users.ToListAsync();

    public async Task AddAsync(User user)
        => await _db.Users.AddAsync(user);

    public async Task DeleteAsync(User user)
        => _db.Users.Remove(user);

    public async Task SaveChangesAsync()
        => await _db.SaveChangesAsync();
}
