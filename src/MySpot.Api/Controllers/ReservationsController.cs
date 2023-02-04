using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private int _id = 1;
    private readonly List<Reservation> _reservations = new();

    private readonly List<string> _parkingSpotName = new()
    {
        "P1", "P2", "P3", "P4", "P5"
    };

    [HttpGet]
    public void Get()
    {
    }
    
    [HttpPost]
    public void Post(Reservation reservation)
    {
        if (_parkingSpotName.All(x => x != reservation.ParkingSpotName))
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var reservationAlreadyExists = _reservations.Any(x =>
            x.ParkingSpotName == reservation.ParkingSpotName &&
            x.Date.Date == reservation.Date.Date);

        if (reservationAlreadyExists)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        reservation.Id = _id;
        reservation.Date = DateTime.UtcNow.AddDays(1);
        _id++;
        
        _reservations.Add(reservation);
        
    }
}
