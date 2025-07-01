using Domain.Entities;

namespace Domain.Interfaces;

public interface IMeetingRepository
{
    Task<Meeting?> GetByIdAsync(int id);
    Task<IEnumerable<Meeting>> GetAllAsync();
    Task<IEnumerable<Meeting>> GetByOrganizerIdAsync(int organizerId);
    Task AddAsync(Meeting meeting);
    Task DeleteAsync(Meeting meeting);
    Task SaveChangesAsync();
}
