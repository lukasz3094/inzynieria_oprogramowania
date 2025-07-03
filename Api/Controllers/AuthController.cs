using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Contracts.DTOs.Api;
using Patterns.Facade;
using Patterns.Adapter;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IOutlookAdapter outlookAdapter) : ControllerBase
{
	private readonly IOutlookAdapter _outlookAdapter = outlookAdapter;

	[HttpGet("outlook/login-url")]
    public IActionResult GetAuthUrl()
    {
        var url = _outlookAdapter.GenerateAuthorizationUrl();
        return Ok(url);
    }
}
