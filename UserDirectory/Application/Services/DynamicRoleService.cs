using AutoMapper;
using UserDirectory.Application.Abstraction.Repositories;
using UserDirectory.Application.Abstraction.Services;
using UserDirectory.Application.Dtos;

namespace UserDirectory.Application.Services;

public class DynamicRoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public DynamicRoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync(CancellationToken ct = default)
    {
        var roles = await _roleRepository.GetAllAsync(ct);
        return roles.Select(r => _mapper.Map<RoleDto>(r));
    }
}