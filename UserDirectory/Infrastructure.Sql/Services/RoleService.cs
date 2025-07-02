using UserDirectory.Application.Dtos;
using UserDirectory.Application.Services;
using UserDirectory.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace UserDirectory.Infrastructure.Sql.Services;

public class RoleService : IRoleService
{
    private readonly UserDirectoryDbContext _context;

    public RoleService(UserDirectoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        return await _context.Roles
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name })
            .ToListAsync();
    }
}
