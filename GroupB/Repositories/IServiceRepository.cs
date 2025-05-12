using GroupB.Models;

namespace GroupB.Repositories;



public interface IServiceRepository
{
    Task<List<Service>> GetServicesByVisitIdAsync(int visitId);
    Task<Service?> GetServiceByNameAsync(string name);
    Task AddVisitServiceAsync(int visitId, int serviceId, decimal fee);

}
