namespace Contracts.DTOs.Api;

public class UserDetailsDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
