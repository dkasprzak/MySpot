using MySpot.Api.ValueObjects;

namespace MySpot.Api.Commands;

public record CreateReservation(Guid ParkingSpotId, Guid ReservationId,  DateTimeOffset Date,
    string EmployeeName, string LicensePlate);
