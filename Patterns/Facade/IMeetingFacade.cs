namespace Patterns.Facade;

using Contracts.DTOs.Api;
using System.Threading.Tasks;

public interface IMeetingFacade
{
	Task<bool> PlanMeetingAsync(int userId, MeetingCreateDto meetingDto);
}
