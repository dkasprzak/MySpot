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
    public ActionResult<IEnumerable<ReservationDto>> Get() => Ok(_service.GetAllWeekly());

    [HttpGet("{id:guid}")]
    public ActionResult<ReservationDto> Get(Guid id)
    {
        var reservation = _service.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(CreateReservation command)
    {
        var id = _service.Create(command with {ReservationId = Guid.NewGuid()});
        if (id is null)
        {
            return BadRequest();
        }
        
        return CreatedAtAction(nameof(Get), new {id}, null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id ,ChangeReservationLicensePlate command)
    {
        if (_service.Update(command with{ReservationId = id}))
        {
            return NoContent();
        }
        
        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id, DeleteReservation command)
    {
        if (_service.Delete(new DeleteReservation(id)))
        {
            return NoContent();
        }
        
        return NotFound();
    }
}
