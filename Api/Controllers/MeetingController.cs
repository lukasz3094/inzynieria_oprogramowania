using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/meetings")]
public class MeetingsController : ControllerBase
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUserRepository _userRepository;

    public MeetingsController(IMeetingRepository meetingRepository, IUserRepository userRepository)
    {
        _meetingRepository = meetingRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var meetings = await _meetingRepository.GetAllAsync();
        return Ok(meetings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var meeting = await _meetingRepository.GetByIdAsync(id);
        if (meeting == null) return NotFound();
        return Ok(meeting);
    }

    [HttpGet("organizer/{organizerId}")]
    public async Task<IActionResult> GetByOrganizer(int organizerId)
    {
        var meetings = await _meetingRepository.GetByOrganizerIdAsync(organizerId);
        return Ok(meetings);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Meeting meeting)
    {
        var organizer = await _userRepository.GetByIdAsync(meeting.OrganizerId);
        if (organizer == null)
        {
            return BadRequest($"Organizer with ID {meeting.OrganizerId} does not exist.");
        }

        await _meetingRepository.AddAsync(meeting);
        await _meetingRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = meeting.Id }, meeting);
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
