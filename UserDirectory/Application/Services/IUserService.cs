using UserDirectory.Application.Dtos;

namespace UserDirectory.Application.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken ct = default);
    Task<UserDto?> GetUserAsync(int id, CancellationToken ct = default);
    Task<UserDto> CreateUserAsync(CreateUserDto dto, CancellationToken ct = default);
    Task<UserDto?> UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(int id, CancellationToken ct = default);
}
