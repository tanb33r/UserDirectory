using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using UserDirectory.Application.Dtos;
using AutoMapper;

namespace UserDirectory.WebApi.IntegrationTests;

public class UserSearchIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserSearchIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchUsers_ReturnsMatchingUsers()
    {
        // Arrange
        var client = _factory.CreateClient();
        var searchQuery = "john";

        // Act
        var response = await client.GetAsync($"/users/search?q={searchQuery}");

        // Assert
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<UserDto[]>();
        Assert.NotNull(users);
        Assert.Contains(users!, u => (u.FirstName + u.LastName).ToLower().Contains(searchQuery));
    }

    [Fact]
    public async Task SearchUsers_EmptyQuery_ReturnsAllUsers()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/users/search");

        // Assert
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<UserDto[]>();
        Assert.NotNull(users);
        Assert.True(users!.Length > 0);
    }
}
