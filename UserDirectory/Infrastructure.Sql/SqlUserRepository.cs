using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Sql;

public class SqlUserRepository : IUserRepository
{
    private readonly UserDirectoryDbContext _dbContext;

    public SqlUserRepository(UserDirectoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Contact)
            .ToListAsync(ct);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Contact)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        await _dbContext.Users.AddAsync(user, ct);
        await _dbContext.SaveChangesAsync(ct);
        return user;
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FindAsync(new object[] { id }, ct);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
