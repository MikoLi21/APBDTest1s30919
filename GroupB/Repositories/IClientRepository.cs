using GroupB.Models;

namespace GroupB.Repositories;

public interface IClientRepository
{
    Task<Client?> GetClientByIdAsync(int clientId);
}