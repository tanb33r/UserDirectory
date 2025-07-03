using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Mongo.Repositories
{
    public class MongoUserRepositoryAsync : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public MongoUserRepositoryAsync(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            _users = database.GetCollection<User>("users2");
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
            // Find max Id in the collection
            var maxUser = await _users.Find(_ => true)
                .SortByDescending(u => u.Id)
                .Limit(1)
                .FirstOrDefaultAsync(ct);
            user.Id = maxUser != null ? maxUser.Id + 1 : 1;

            // Assign max(id)+1 for Contact if present, using all contacts from users
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
    }
}
