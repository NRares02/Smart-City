using MongoDB.Driver;
using SmartCity.Api.Models;

namespace SmartCity.Api.Repositories;

public class RoleRepository
{
    private readonly IMongoCollection<Role> _roles;

    public RoleRepository(IMongoDatabase database)
    {
        _roles = database.GetCollection<Role>("roles");
    }

    public async Task<Role?> GetByIdAsync(string id)
    {
        return await _roles.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _roles.Find(r => r.Name == name).FirstOrDefaultAsync();
    }
}
