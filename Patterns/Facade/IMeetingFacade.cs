namespace Patterns.Facade
{
	using Api.DTOs.Api;
	using Domain.Entities;
    using System.Threading.Tasks;

    public interface IMeetingFacade
    {
        Task<bool> PlanMeetingAsync(int userId, MeetingCreateDto meetingDto);
    }
}
