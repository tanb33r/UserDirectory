using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _sqlRepo;
    private readonly IUserRepository _mongoRepo;
    private readonly IRoleService _sqlRoleService;
    private readonly IRoleService _mongoRoleService;

    public DynamicUserService(
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IUserRepository sqlRepo,
        IUserRepository mongoRepo,
        IRoleService sqlRoleService,
        IRoleService mongoRoleService)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _sqlRepo = sqlRepo;
        _mongoRepo = mongoRepo;
        _sqlRoleService = sqlRoleService;
        _mongoRoleService = mongoRoleService;
    }

    private string GetDataSource()
    {
        var ds = _httpContextAccessor.HttpContext?.Request.Headers["X-Data-Source"].ToString();
        return string.IsNullOrWhiteSpace(ds) ? "MSSMS" : ds;
    }

    private IUserRepository GetRepo() => GetDataSource() == "MongoDB" ? _mongoRepo : _sqlRepo;
    private IRoleService GetRoleService() => GetDataSource() == "MongoDB" ? _mongoRoleService : _sqlRoleService;

    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var repo = GetRepo();
        var users = await repo.GetAllAsync(ct);
        var roles = (await GetRoleService().GetRolesAsync()).ToList();
        return users.Select(u => {
            var dto = _mapper.Map<UserDto>(u);
            var role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            dto.Role = role;
            return dto;
        });
    }

    public async Task<UserDto?> GetUserAsync(int id, CancellationToken ct = default)
    {
        var repo = GetRepo();
        var user = await repo.GetByIdAsync(id, ct);
        if (user == null) return null;
        var role = (await GetRoleService().GetRolesAsync()).FirstOrDefault(r => r.Id == user.RoleId);
        var dto = _mapper.Map<UserDto>(user);
        dto.Role = role;
        return dto;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var repo = GetRepo();
        var user = _mapper.Map<User>(dto);
        var created = await repo.CreateAsync(user, ct);
        var role = (await GetRoleService().GetRolesAsync()).FirstOrDefault(r => r.Id == created.RoleId);
        var userDto = _mapper.Map<UserDto>(created);
        userDto.Role = role;
        return userDto;
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default)
    {
        var repo = GetRepo();
        var user = _mapper.Map<User>(dto);
        await repo.UpdateAsync(user, ct);
        var role = (await GetRoleService().GetRolesAsync()).FirstOrDefault(r => r.Id == user.RoleId);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = role;
        return userDto;
    }

    public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default)
    {
        var repo = GetRepo();
        await repo.DeleteAsync(id, ct);
        return true;
    }

    public async Task<IEnumerable<UserDto>> SearchUsersAsync(string? search, CancellationToken ct = default)
    {
        var repo = GetRepo();
        var users = await repo.SearchAsync(search, ct);
        var roles = (await GetRoleService().GetRolesAsync()).ToList();
        return users.Select(u => {
            var dto = _mapper.Map<UserDto>(u);
            var role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            dto.Role = role;
            return dto;
        });
    }
}
