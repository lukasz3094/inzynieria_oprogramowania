using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Contracts.DTOs.Api;
using Patterns.Facade;

namespace Api.Controllers;

[ApiController]
[Route("api/meetings")]
public class MeetingsController(IMeetingRepository meetingRepository, IUserRepository userRepository, ISchedulingStrategyFactory schedulingStrategyFactory) : ControllerBase
{
    private readonly IMeetingRepository _meetingRepository = meetingRepository;
    private readonly IUserRepository _userRepository = userRepository;
	private readonly ISchedulingStrategyFactory _strategyFactory = schedulingStrategyFactory;
	private readonly IMeetingFacade _meetingFacade = new MeetingFacade(userRepository, schedulingStrategyFactory, meetingRepository);

	[HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var meetings = await _meetingRepository.GetAllAsync();

		var result = meetings.Select(m => new MeetingDetailsDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            StartTime = m.StartTime,
            EndTime = m.EndTime,
            Location = m.Location,
            OrganizerId = m.OrganizerId,
            OrganizerName = m.Organizer.FullName,
            Attendees = [.. m.Attendees.Select(a => new UserDetailsDto
            {
                Id = a.User.Id,
                Email = a.User.Email,
                FullName = a.User.FullName
            })]
		});

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var meeting = await _meetingRepository.GetByIdAsync(id);
        if (meeting == null) return NotFound();

		var result = new MeetingDetailsDto
        {
            Id = meeting.Id,
            Title = meeting.Title,
            Description = meeting.Description,
            StartTime = meeting.StartTime,
            EndTime = meeting.EndTime,
            Location = meeting.Location,
            OrganizerId = meeting.OrganizerId,
            OrganizerName = meeting.Organizer.FullName,
            Attendees = [.. meeting.Attendees.Select(a => new UserDetailsDto
            {
                Id = a.User.Id,
                Email = a.User.Email,
                FullName = a.User.FullName
            })]
		};
		
        return Ok(result);
    }

    [HttpGet("organizer/{organizerId}")]
    public async Task<IActionResult> GetByOrganizer(int organizerId)
    {
        var meetings = await _meetingRepository.GetByOrganizerIdAsync(organizerId);

		var result = meetings.Select(m => new MeetingDetailsDto
		{
			Id = m.Id,
			Title = m.Title,
			Description = m.Description,
			StartTime = m.StartTime,
			EndTime = m.EndTime,
			Location = m.Location,
			OrganizerId = m.OrganizerId,
			OrganizerName = m.Organizer.FullName,
			Attendees = [.. m.Attendees.Select(a => new UserDetailsDto
			{
				Id = a.User.Id,
				Email = a.User.Email,
				FullName = a.User.FullName
			})]
		});

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MeetingCreateDto dto)
    {
        var organizer = await _userRepository.GetByIdAsync(dto.OrganizerId);
        if (organizer == null)
            return BadRequest($"Organizer with ID {dto.OrganizerId} does not exist.");

		var meeting = new Meeting
        {
            Title = dto.Title,
            Description = dto.Description,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Location = dto.Location,
            OrganizerId = dto.OrganizerId
        };

        await _meetingRepository.AddAsync(meeting);
        await _meetingRepository.SaveChangesAsync();

		var result = new MeetingDetailsDto
		{
			Id = meeting.Id,
			Title = meeting.Title,
			Description = meeting.Description,
			StartTime = meeting.StartTime,
			EndTime = meeting.EndTime,
			Location = meeting.Location,
			OrganizerId = meeting.OrganizerId,
			OrganizerName = organizer.FullName,
			Attendees = []
		};

        return CreatedAtAction(nameof(GetById), new { id = meeting.Id }, result);
    }

	[HttpPost("plan")]
    public async Task<IActionResult> Plan([FromBody] MeetingCreateDto dto, [FromQuery] int userId)
    {
        var result = await _meetingFacade.PlanMeetingAsync(userId, dto);
        return result ? Ok("Meeting planned.") : BadRequest("Could not plan meeting.");
    }

	[HttpPost("{meetingId}/attendees")]
	public async Task<IActionResult> AddAttendee(int meetingId, [FromQuery] int userId)
	{
		var meeting = await _meetingRepository.GetByIdAsync(meetingId);
		if (meeting == null)
			return NotFound($"Meeting with ID {meetingId} not found.");

		var user = await _userRepository.GetByIdAsync(userId);
		if (user == null)
			return NotFound($"User with ID {userId} not found.");

		var isAlreadyAttendee = meeting.Attendees.Any(a => a.UserId == userId);
		if (isAlreadyAttendee)
			return BadRequest("User is already an attendee of this meeting.");

		meeting.Attendees.Add(new MeetingAttendee
		{
			MeetingId = meetingId,
			UserId = userId
		});

		await _meetingRepository.SaveChangesAsync();

		return Ok("User added to meeting.");
	}

	[HttpDelete("{meetingId}/attendees")]
	public async Task<IActionResult> RemoveAttendee(int meetingId, [FromQuery] int userId)
	{
		var meeting = await _meetingRepository.GetByIdAsync(meetingId);
		if (meeting == null)
			return NotFound($"Meeting with ID {meetingId} not found.");

		var attendee = meeting.Attendees.FirstOrDefault(a => a.UserId == userId);
		if (attendee == null)
			return NotFound("User is not an attendee of this meeting.");

		meeting.Attendees.Remove(attendee);
		await _meetingRepository.SaveChangesAsync();

		return Ok("User removed from meeting.");
	}

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var meeting = await _meetingRepository.GetByIdAsync(id);
        if (meeting == null) return NotFound();

        await _meetingRepository.DeleteAsync(meeting);
        await _meetingRepository.SaveChangesAsync();

        return NoContent();
    }	
}
