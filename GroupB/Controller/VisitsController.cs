using GroupB.Dtos;
using GroupB.DTOs;
using GroupB.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupB.Controller;

[ApiController]
[Route("api/visits")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitsController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet("{id}"]
    public async Task<ActionResult<VisitResponseDto>> GetVisitById(int id)
    {
        var visit=await _visitService.GetVisitDetailsAsync(id);
        if ()
    }
    
    
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitResponseDto>> GetVisitById(int id)
    {
        var visit = await _visitService.GetVisitDetailsAsync(id);
        if (visit == null)
            return NotFound();

        return Ok(visit);
    }

    [HttpPost]
    public async Task<IActionResult> AddVisit([FromBody] VisitRequestDto request)
    {
        var result = await _visitService.AddVisitAsync(request);

        if (result == null)
            return CreatedAtAction(nameof(GetVisitById), new { id = request.VisitId }, null);

        return BadRequest(new { error = result });
    }
}