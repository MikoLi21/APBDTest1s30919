using GroupB.Dtos;
using GroupB.DTOs;

namespace GroupB.Services;




public interface IVisitService
{
    Task<VisitResponseDto?> GetVisitDetailsAsync(int id);
    Task<string?> AddVisitAsync(VisitRequestDto request);

}
