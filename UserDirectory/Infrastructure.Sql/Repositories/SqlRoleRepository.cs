using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Abstraction.Repositories;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Sql.Repositories;

public class SqlRoleRepository : IRoleRepository
{
    private readonly UserDirectoryDbContext _context;

    public SqlRoleRepository(UserDirectoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Roles.ToListAsync(ct);
    }
}