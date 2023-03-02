﻿using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    [Fact]
    public void create_reservation_for_not_taken_date_add_reservation_should_succeed()
    {
        // ARRANGE
        var weeklyParkingSpot = _weeklyParkingSpotRepository.GetAll().First();
        var command = new CreateReservation(weeklyParkingSpot.Id, Guid.NewGuid(), 
            DateTime.UtcNow.AddMinutes(5), "John Doe", "XYZ123");

        // ACT
        var reservationId = _reservationsService.Create(command);
        
        // ASSERT
        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }

    #region Arrange

    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IReservationsService _reservationsService;

    public ReservationServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationsService = new ReservationsService(_clock, _weeklyParkingSpotRepository);
    }
    
    #endregion
}
