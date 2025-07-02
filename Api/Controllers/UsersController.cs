using Contracts.DTOs.Api;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;

	[HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
		var result = users.Select(u => new UserDetailsDto
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName
        });

        return Ok(result);
    }

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var user = await _userRepository.GetByIdAsync(id);
		if (user == null) return NotFound();
        
		return Ok(new UserDetailsDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            FullName = dto.FullName
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return NotFound();

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();

        return NoContent();
    }
}
