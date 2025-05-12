using GroupB.Models;
namespace GroupB.Repositories;

public interface IVisitRepository
{
    Task<Visit?> GetVisitByIdAsync(int id);
    Task<bool> VisitExistsAsync(int visitId);
    Task AddVisitAsync(Visit visit);

}
