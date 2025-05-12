using GroupB.Models;

namespace GroupB.Repositories;


public interface IMechanicRepository
{
    Task<Mechanic?> GetMechanicByIdAsync(int mechanicId);
    Task<Mechanic?> GetMechanicByLicenceAsync(string licenceNumber);

}
