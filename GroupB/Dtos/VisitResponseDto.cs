namespace GroupB.Dtos;

public class VisitResponseDto
{
    public DateTime Date { get; set; }
    public ClientDto Client { get; set; } = new();
    public MechanicDto Mechanic { get; set; } = new();
    public List<ServiceResponseDto> VisitServices { get; set; } = new();
}
