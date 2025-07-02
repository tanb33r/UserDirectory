using System.Text.Json;
using Microsoft.Extensions.Configuration;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain;

namespace UserDirectory.Infrastructure.Json;

internal class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _opts = new() { WriteIndented = true };
    private readonly SemaphoreSlim _gate = new(1, 1);

    public JsonUserRepository(IConfiguration cfg)
    {
        _filePath = cfg.GetValue<string>("JsonStore:File") ?? "users.json";
    }

    private async Task<List<User>> Load()
    {
        if (!File.Exists(_filePath)) return [];
        await using var fs = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<User>>(fs) ?? [];
    }

    private async Task Save(List<User> users)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await using var fs = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(fs, users, _opts);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct);
        try { return await Load(); }
        finally { _gate.Release(); }
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct);
        try { return (await Load()).FirstOrDefault(u => u.Id == id); }
        finally { _gate.Release(); }
    }

    public async Task<User> CreateAsync(User user, CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct);
        try
        {
            var list = await Load();
            user.Id = list.Any() ? list.Max(u => u.Id) + 1 : 1;
            list.Add(user);
            await Save(list);
            return user;
        }
        finally { _gate.Release(); }
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct);
        try
        {
            var list = await Load();
            var idx = list.FindIndex(u => u.Id == user.Id);
            if (idx >= 0) list[idx] = user;
            await Save(list);
        }
        finally { _gate.Release(); }
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct);
        try
        {
            var list = await Load();
            list.RemoveAll(u => u.Id == id);
            await Save(list);
        }
        finally { _gate.Release(); }
    }
}
