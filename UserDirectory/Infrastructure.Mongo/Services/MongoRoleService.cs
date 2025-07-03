using MongoDB.Driver;
using UserDirectory.Application.Dtos;
using UserDirectory.Application.Services;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Mongo.Services;

public class MongoRoleService : IRoleService
{
    private readonly IMongoCollection<Role> _roles;

    public MongoRoleService(string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);
        _roles = database.GetCollection<Role>("roles");
    }

    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        var roles = await _roles.Find(_ => true).ToListAsync();
        return roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name });
    }
}
