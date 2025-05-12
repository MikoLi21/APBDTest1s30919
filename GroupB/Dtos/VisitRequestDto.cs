namespace GroupB.DTOs;

public class VisitRequestDto
{
    public int VisitId { get; set; }
    public int ClientId { get; set; }
    public string MechanicLicenceNumber { get; set; } = string.Empty;

    public List<VisitServiceDto> Services { get; set; } = new();
}