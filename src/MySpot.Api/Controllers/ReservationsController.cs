using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationsService _service;

    public ReservationsController(IReservationsService service)
    {
        _service = service;
    }
 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> Get() => 
        Ok(await _service.GetAllWeeklyAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto>> Get(Guid id)
    {
        var reservation = await _service.GetAsync(id);
        if (reservation is null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult> Post(CreateReservation command)
    {
        var id = await _service.CreateAsync(command with {ReservationId = Guid.NewGuid()});
        if (id is null)
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(Get), new {id}, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id ,ChangeReservationLicensePlate command)
    {
        if (await _service.UpdateAsync(command with{ReservationId = id}))
        {
            return NoContent();
        }
        
        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (await _service.DeleteAsync(new DeleteReservation(id)))
        {
            return NoContent();
        }
        
        return NotFound();
    }
}
