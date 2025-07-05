using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserDirectory.Application.Dtos;

namespace UserDirectory.Application.Abstraction.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken ct = default);
}
