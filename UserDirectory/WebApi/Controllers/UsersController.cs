using Microsoft.AspNetCore.Mvc;
using UserDirectory.Application.Abstraction.Services;
using UserDirectory.Application.Dtos;

namespace UserDirectory.WebApi.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetAll() =>
        await _service.GetUsersAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _service.GetUserAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
    {
        var created = await _service.CreateUserAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> Update(UpdateUserDto dto)
    {
        var updated = await _service.UpdateUserAsync(dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removed = await _service.DeleteUserAsync(id);
        return removed ? NoContent() : NotFound();
    }

    [HttpGet("search")]
    public async Task<IEnumerable<UserDto>> Search([FromQuery] string? q)
    => await _service.SearchUsersAsync(q);
}
