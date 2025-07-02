using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Domain.Entities;
using System.Net.Http.Json;
using Contracts.DTOs.Api;

namespace ApiTests;

public class MeetingsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client = factory.CreateClient();

	[Fact]
	public async Task Can_Create_And_Get_Meeting()
	{
		var user = new User { Email = "organizer@example.com", FullName = "Organizer" };
		var userResponse = await _client.PostAsJsonAsync("/api/users", user);
		userResponse.EnsureSuccessStatusCode();
		var createdUser = await userResponse.Content.ReadFromJsonAsync<User>();

		var meeting = new MeetingCreateDto
		{
			Title = "Test Meeting",
			OrganizerId = createdUser!.Id,
			StartTime = DateTime.Now.AddHours(1),
			EndTime = DateTime.Now.AddHours(2)
		};

		var meetingResponse = await _client.PostAsJsonAsync("/api/meetings", meeting);

		if (!meetingResponse.IsSuccessStatusCode)
		{
			var error = await meetingResponse.Content.ReadAsStringAsync();
			Console.WriteLine($"❌ Error creating meeting: {error}");
		}
		meetingResponse.EnsureSuccessStatusCode();
		var createdMeeting = await meetingResponse.Content.ReadFromJsonAsync<Meeting>();

		var getResponse = await _client.GetAsync($"/api/meetings/{createdMeeting!.Id}");
		getResponse.EnsureSuccessStatusCode();

		var fetchedMeeting = await getResponse.Content.ReadFromJsonAsync<Meeting>();

		fetchedMeeting.Should().NotBeNull();
		fetchedMeeting!.Title.Should().Be("Test Meeting");
		fetchedMeeting.OrganizerId.Should().Be(createdUser.Id);
	}
}
