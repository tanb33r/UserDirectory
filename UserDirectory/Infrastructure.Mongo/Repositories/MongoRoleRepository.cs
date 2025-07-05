using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserDirectory.Application.Abstraction.Repositories;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Mongo.Repositories;

public class MongoRoleRepository : IRoleRepository
{
    private readonly IMongoCollection<Role> _roles;

    public MongoRoleRepository(string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);
        _roles = database.GetCollection<Role>("roles");
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default)
    {
        return await _roles.Find(_ => true).ToListAsync(ct);
    }
}