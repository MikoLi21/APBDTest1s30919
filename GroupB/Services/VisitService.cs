using GroupB.Repositories;
using GroupB.Dtos;
using GroupB.DTOs;
using GroupB.Models;

namespace GroupB.Services;


public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IServiceRepository _serviceRepository;

    public VisitService(
        IVisitRepository visitRepository,
        IClientRepository clientRepository,IMechanicRepository mechanicRepository,IServiceRepository serviceRepository)
    {
        _visitRepository = visitRepository;
        _clientRepository = clientRepository;
        _mechanicRepository = mechanicRepository;
        _serviceRepository = serviceRepository;
    }


    public  async Task<VisitResponseDto?> GetVisitDetailsAsync(int id)
    {
       
        var visit = await _visitRepository.GetVisitByIdAsync(id);
        if (visit == null)
            return null;

        var client = await _clientRepository.GetClientByIdAsync(visit.ClientId);
        if (client == null)
            return null; 
        
        var mechanic = await _mechanicRepository.GetMechanicByIdAsync(visit.MechanicId);
        if (mechanic == null)
            return null;
        
        var service = await _serviceRepository.GetServicesByVisitIdAsync(visit.VisitId);
        

                
        return new VisitResponseDto
        {
            Date = visit.Date,
            Client = new ClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                DateOfBirth = client.DateOfBirth
            },
            Mechanic = new MechanicDto
            {
                MechanicId = mechanic.MechanicId, 
                LicenceNumber = mechanic.LicenceNumber,
            },
            VisitServices =service.Select(s=>new ServiceResponseDto
            {
                Name = s.Name,
                ServiceFee = s.ServiceFee,
            }
                ).ToList()
        }; 
    }
    

    public async Task<string?> AddVisitAsync(VisitRequestDto request)
    {
        
        if (await _visitRepository.VisitExistsAsync(request.VisitId))
            return "Visit with this ID already exists.";

        
        var client = await _clientRepository.GetClientByIdAsync(request.ClientId);
        if (client == null)
            return "Client does not exist.";

        
        var mechanic = await _mechanicRepository.GetMechanicByLicenceAsync(request.MechanicLicenceNumber);
        if (mechanic == null)
            return "Mechanic with given licence number does not exist.";

        
        var serviceResults = new List<(int serviceId, decimal fee)>();

        foreach (var s in request.Services)
        {
            var service = await _serviceRepository.GetServiceByNameAsync(s.ServiceName);
            if (service == null)
                return $"Service not found: {s.ServiceName}";

            serviceResults.Add((service.ServiceId, s.ServiceFee));
        }

        
        var visit = new Visit
        {
            VisitId = request.VisitId,
            ClientId = client.ClientId,
            MechanicId = mechanic.MechanicId,
            Date = DateTime.Now
        };
        await _visitRepository.AddVisitAsync(visit);
        
        foreach (var sr in serviceResults)
        {
            await _serviceRepository.AddVisitServiceAsync(request.VisitId, sr.serviceId, sr.fee);
        }

        return null; 
    }
}


