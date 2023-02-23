using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects;

public record ParkingSpotName
{
    public string Value { get; }

    public ParkingSpotName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidParkingSpotNameException();
        }
        
        Value = value;
    }

    public static implicit operator string(ParkingSpotName parkingSpotName) => parkingSpotName.Value;
    public static implicit operator ParkingSpotName(string parkingSpotName) => new(parkingSpotName);
}
