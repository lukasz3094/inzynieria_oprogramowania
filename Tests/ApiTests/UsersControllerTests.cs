using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Domain.Entities;
using System.Net.Http.Json;
using Contracts.DTOs.Api;

namespace ApiTests;

public class UsersControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

	[Fact]
    public async Task Can_Create_And_Get_User()
    {
        var user = new UserCreateDto
        {
            Email = "test@example.com",
            FullName = "Test User"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/users", user);
        createResponse.EnsureSuccessStatusCode();

        var createdUser = await createResponse.Content.ReadFromJsonAsync<User>();
		Console.WriteLine($"✅ Created User ID: {createdUser!.Id}");

		var getResponse = await _client.GetAsync($"/api/users/{createdUser!.Id}");
        getResponse.EnsureSuccessStatusCode();

        var fetchedUser = await getResponse.Content.ReadFromJsonAsync<User>();

		createdUser.Id.Should().BeGreaterThan(0);
		fetchedUser.Should().NotBeNull();
        fetchedUser!.Email.Should().Be("test@example.com");
        fetchedUser.FullName.Should().Be("Test User");
    }
}
