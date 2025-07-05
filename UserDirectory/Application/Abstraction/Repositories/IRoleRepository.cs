using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UserDirectory.Domain;

namespace UserDirectory.Application.Abstraction.Repositories;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default);
}