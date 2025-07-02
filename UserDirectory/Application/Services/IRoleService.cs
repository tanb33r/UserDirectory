using UserDirectory.Application.Dtos;

namespace UserDirectory.Application.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetRolesAsync();
}
