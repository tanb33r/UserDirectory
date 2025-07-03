using AutoMapper;
using MongoDB.Driver;
using UserDirectory.Application.Dtos;
using UserDirectory.Application.Interfaces;
using UserDirectory.Application.Services;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Mongo.Services;

public sealed class MongoUserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    private readonly IMongoCollection<Role> _roles;

    public MongoUserService(IUserRepository repo, IMapper mapper, string connectionString, string dbName)
    {
        _repo = repo;
        _mapper = mapper;
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);
        _roles = database.GetCollection<Role>("roles");
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _repo.GetAllAsync(ct);
        var roles = await _roles.Find(_ => true).ToListAsync(ct);
        return users.Select(u =>
        {
            var dto = _mapper.Map<UserDto>(u);
            var role = roles.FirstOrDefault(r => r.Id == u.RoleId);
            dto.Role = role != null ? new RoleDto { Id = role.Id, Name = role.Name } : null!;
            return dto;
        });
    }

    public async Task<UserDto?> GetUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return null;
        var role = await _roles.Find(r => r.Id == user.RoleId).FirstOrDefaultAsync(ct);
        var dto = _mapper.Map<UserDto>(user);
        dto.Role = role != null ? new RoleDto { Id = role.Id, Name = role.Name } : null!;
        return dto;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var user = _mapper.Map<User>(dto);
        var created = await _repo.CreateAsync(user, ct);
        var role = await _roles.Find(r => r.Id == created.RoleId).FirstOrDefaultAsync(ct);
        var userDto = _mapper.Map<UserDto>(created);
        userDto.Role = role != null ? new RoleDto { Id = role.Id, Name = role.Name } : null!;
        return userDto;
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = _mapper.Map<User>(dto);
        await _repo.UpdateAsync(user, ct);
        var role = await _roles.Find(r => r.Id == user.RoleId).FirstOrDefaultAsync(ct);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = role != null ? new RoleDto { Id = role.Id, Name = role.Name } : null!;
        return userDto;
    }

    public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default)
    {
        await _repo.DeleteAsync(id, ct);
        return true;
    }
}
