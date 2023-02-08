namespace MySpot.Api.Exceptions;

public class ParkingSpotNameAlreadyReservedException : CustomException
{
    public string Name { get; }
    public DateTime Date { get; }
    
    public ParkingSpotNameAlreadyReservedException(string name ,DateTime date) : base($"Parking spot: {name} is already reserved at: {date:d}.")
    {
        Name = name;
        Date = date;
    }
}