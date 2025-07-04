using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Dtos;
using UserDirectory.Application.Services;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Sql.Services;

public sealed class UserService : IUserService
{
    private readonly UserDirectoryDbContext _db;
    private readonly IMapper _mapper;

    public UserService(UserDirectoryDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct = default)
        => await _db.Users
                    .Include(u => u.Contact)
                    .Include(u => u.Role)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

    public async Task<UserDto?> GetUserAsync(int id, CancellationToken ct = default)
        => await _db.Users
                    .Include(u => u.Contact)
                    .Include(u => u.Role)
                    .Where(u => u.Id == id)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(ct);

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default)
    {
        var user = _mapper.Map<User>(dto);
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default)
    {
        var user = await _db.Users
                            .Include(u => u.Contact)
                            .SingleOrDefaultAsync(u => u.Id == dto.Id, ct);

        if (user is null)
            return null;

        _mapper.Map(dto, user);
        await _db.SaveChangesAsync(ct);

        var role = await _db.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId, ct);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = role != null ? new RoleDto { Id = role.Id, Name = role.Name } : null!;
        return userDto;
    }

    public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _db.Users.FindAsync(new object[] { id }, ct);
        if (user is null) return false;
        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
