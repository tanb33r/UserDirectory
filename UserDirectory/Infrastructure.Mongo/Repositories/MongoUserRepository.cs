using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserDirectory.Application.Abstraction.Repositories;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Mongo.Repositories
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public MongoUserRepository(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            _users = database.GetCollection<User>("users");
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _users.Find(_ => true).ToListAsync(ct);
            return result;
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync(ct);
        }

        public async Task<User> CreateAsync(User user, CancellationToken ct = default)
        {
            var maxUser = await _users.Find(_ => true)
                .SortByDescending(u => u.Id)
                .Limit(1)
                .FirstOrDefaultAsync(ct);
            user.Id = maxUser != null ? maxUser.Id + 1 : 1;

            if (user.Contact != null)
            {
                var allContacts = await _users.Find(_ => true)
                    .Project(u => u.Contact)
                    .ToListAsync(ct);
                var maxContactId = allContacts
                    .Where(c => c != null)
                    .Select(c => c.Id)
                    .DefaultIfEmpty(0)
                    .Max();
                user.Contact.Id = maxContactId + 1;
            }
            
            await _users.InsertOneAsync(user, cancellationToken: ct);
            return user;
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _users.DeleteOneAsync(u => u.Id == id, ct);
        }

        public async Task<IEnumerable<User>> SearchAsync(string? search, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return await _users.Find(_ => true).ToListAsync(ct);
            }

            var lowered = search.Replace(" ", "").ToLower();

            // MongoDB does not support string concatenation in queries, so we filter by first and last name in the DB,
            // then do the concatenation check in memory.
            var users = await _users.Find(
                Builders<User>.Filter.Or(
                    Builders<User>.Filter.Regex(u => u.FirstName, new MongoDB.Bson.BsonRegularExpression(lowered, "i")),
                    Builders<User>.Filter.Regex(u => u.LastName, new MongoDB.Bson.BsonRegularExpression(lowered, "i"))
                )
            ).ToListAsync(ct);

            // In-memory filtering for concatenated names
            users = users
                .Where(u =>
                    (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.Replace(" ", "").ToLower().Contains(lowered)) ||
                    (!string.IsNullOrEmpty(u.LastName) && u.LastName.Replace(" ", "").ToLower().Contains(lowered)) ||
                    (!string.IsNullOrEmpty(u.FirstName) && !string.IsNullOrEmpty(u.LastName) &&
                        (u.FirstName + u.LastName).Replace(" ", "").ToLower().Contains(lowered))
                )
                .ToList();

            return users;
        }
    }
}
