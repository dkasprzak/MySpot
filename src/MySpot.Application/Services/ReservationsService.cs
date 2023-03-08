using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService : IReservationsService
{
    private readonly IClock _clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotsSpotRepository;

    public ReservationsService(IClock clock , IWeeklyParkingSpotRepository weeklyParkingSpotsSpotRepository)
    {
        _clock = clock;
        _weeklyParkingSpotsSpotRepository = weeklyParkingSpotsSpotRepository;
    }

    public async Task<ReservationDto> GetAsync(Guid id)
    {
        var reservations = await GetAllWeeklyAsync();
        return reservations.SingleOrDefault(x => x.Id == id);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
    {
        var weeklyParkingSpots = await _weeklyParkingSpotsSpotRepository.GetAllAsync();
            
            return weeklyParkingSpots
            .SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });
    }

    public async Task<Guid?> CreateAsync(CreateReservation command)
    {
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var weeklyParkingSpot = await _weeklyParkingSpotsSpotRepository.GetAsync(parkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName,
            command.LicensePlate, command.Date);
        weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));
        await _weeklyParkingSpotsSpotRepository.UpdateAsync(weeklyParkingSpot);
        
        return reservation.Id;
    }

    public async Task<bool> UpdateAsync(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        if (existingReservation.Date.Value.Date <= _clock.Current())
        {
            return false;
        }

        existingReservation.ChangeLicensePlate(command.LicensePlate);
        await _weeklyParkingSpotsSpotRepository.UpdateAsync(weeklyParkingSpot);
        return true;
    }

    public async Task<bool> DeleteAsync(DeleteReservation command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);
        if (weeklyParkingSpot is null)
        {
            return false;
        }
        
        var existingReservation = weeklyParkingSpot.Reservations.SingleOrDefault(x => x.Id == command.ReservationId);
        if (existingReservation is null)
        {
            return false;
        }

        weeklyParkingSpot.RemoveReservation(command.ReservationId);
        await _weeklyParkingSpotsSpotRepository.DeleteAsync(weeklyParkingSpot);
        return true;
    }

    private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await _weeklyParkingSpotsSpotRepository.GetAllAsync();
        return weeklyParkingSpots.SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId)); 
    }
}
