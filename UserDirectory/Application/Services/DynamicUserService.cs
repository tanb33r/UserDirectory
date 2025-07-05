using AutoMapper;
using Microsoft.AspNetCore.Http;
using UserDirectory.Application.Abstraction.Repositories;
using UserDirectory.Application.Abstraction.Services;
using UserDirectory.Application.Dtos;
using UserDirectory.Domain;

namespace UserDirectory.Application.Services;

public class DynamicUserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly IRoleService _userRoleService;

    public DynamicUserService(
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepo,
        IRoleService userRoleService)
    {
        _mapper = mapper;
        _userRepo = userRepo;
        _userRoleService = userRoleService;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _userRepo.GetAllAsync(ct);
        var roles = (await _userRoleService.GetRolesAsync()).ToList();
        return users.Select(u =>
        {
            var dto = _mapper.Map<UserDto>(u);
            var role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            dto.Role = role!;
            return dto;
        });
    }

    public async Task<UserDto?> GetUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _userRepo.GetByIdAsync(id, ct);
        if (user == null) return null;
        var role = (await _userRoleService.GetRolesAsync()).FirstOrDefault(r => r.Id == user.RoleId);
        var dto = _mapper.Map<UserDto>(user);
        dto.Role = role!;
        return dto;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var user = _mapper.Map<User>(dto);
        var created = await _userRepo.CreateAsync(user, ct);
        var role = (await _userRoleService.GetRolesAsync()).FirstOrDefault(r => r.Id == created.RoleId);
        var userDto = _mapper.Map<UserDto>(created);
        userDto.Role = role!;
        return userDto;
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = _mapper.Map<User>(dto);
        await _userRepo.UpdateAsync(user, ct);
        var role = (await _userRoleService.GetRolesAsync()).FirstOrDefault(r => r.Id == user.RoleId);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = role!;
        return userDto;
    }

    public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default)
    {
        await _userRepo.DeleteAsync(id, ct);
        return true;
    }

    public async Task<IEnumerable<UserDto>> SearchUsersAsync(string? search, CancellationToken ct = default)
    {
        var users = await _userRepo.SearchAsync(search, ct);
        var roles = (await _userRoleService.GetRolesAsync()).ToList();

        return users.Select(u =>
        {
            var dto = _mapper.Map<UserDto>(u);
            var role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            dto.Role = role!;
            return dto;
        });
    }
}
