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
        var existingUser = await _dbContext.Users
            .Include(u => u.Contact)
            .FirstOrDefaultAsync(u => u.Id == user.Id, ct);

        if (existingUser == null)
            throw new InvalidOperationException("User not found");

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Active = user.Active;
        existingUser.Company = user.Company;
        existingUser.Sex = user.Sex;
        existingUser.RoleId = user.RoleId;

        if (user.Contact != null && existingUser.Contact != null)
        {
            existingUser.Contact.Phone = user.Contact.Phone;
            existingUser.Contact.Address = user.Contact.Address;
            existingUser.Contact.City = user.Contact.City;
            existingUser.Contact.Country = user.Contact.Country;
        }

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

    public async Task<IEnumerable<User>> SearchAsync(string? search, CancellationToken ct = default)
    {
        var query = _dbContext.Users
            .Include(u => u.Contact)
            .Include(u => u.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.Replace(" ", "").ToLower();
            query = query.Where(u =>
                u.FirstName.ToLower().Contains(lowered) ||
                u.LastName.ToLower().Contains(lowered) ||
                (u.FirstName + u.LastName).ToLower().Contains(lowered)
            );
        }

        return await query.ToListAsync(ct);
    }
}
