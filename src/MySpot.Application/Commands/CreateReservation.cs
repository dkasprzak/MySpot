namespace MySpot.Application.Commands;

public record CreateReservation(Guid ParkingSpotId, Guid ReservationId,  DateTimeOffset Date,
    string EmployeeName, string LicensePlate);
