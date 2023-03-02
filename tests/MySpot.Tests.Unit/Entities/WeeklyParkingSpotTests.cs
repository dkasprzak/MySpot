using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{
    [Theory]
    [InlineData("2023-01-22")]
    [InlineData("2023-02-07")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        // ARRANGE
        var invalidDate = DateTime.Parse(dateString);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "XYZ122",
            new Date(invalidDate));
        
        // ACT
        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, new Date(_now)));
        
        // ASSERT
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReservationDateException>();
    }

    [Fact]
    public void given_reservation_for_already_existing_date_add_reservation_should_fail()
    {
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "XYZ122",
          reservationDate);
        var nextReservation =  new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "XYZ122",
            reservationDate);
        _weeklyParkingSpot.AddReservation(reservation, _now);
        
        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(nextReservation, new Date(_now)));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ParkingSpotNameAlreadyReservedException>();
    }

    [Fact]
    public void given_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        var reservationDate = _now.AddDays(1);
        var reservation = new Reservation(Guid.NewGuid(), _weeklyParkingSpot.Id, "John Doe", "XYZ122",
            reservationDate);
        
        _weeklyParkingSpot.AddReservation(reservation, _now);

        _weeklyParkingSpot.Reservations.ShouldHaveSingleItem();
    }

    #region Arrange
    
    private readonly Date _now;
    private readonly WeeklyParkingSpot _weeklyParkingSpot;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2023, 01, 23));
        _weeklyParkingSpot = new WeeklyParkingSpot(Guid.NewGuid(), new Week(_now), "P1");
    }
    
    #endregion
}