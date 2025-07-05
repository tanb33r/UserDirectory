using Microsoft.AspNetCore.Mvc;
using UserDirectory.Application.Abstraction.Services;
using UserDirectory.Application.Dtos;

namespace UserDirectory.WebApi.Controllers;

[ApiController]
[Route("roles")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service) => _service = service;

    [HttpGet]
    public async Task<IEnumerable<RoleDto>> GetAll() =>
        await _service.GetRolesAsync();
}
