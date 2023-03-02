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

    public ReservationDto Get(Guid id)
        => GetAllWeekly().SingleOrDefault(x => x.Id == id);

    public IEnumerable<ReservationDto> GetAllWeekly() 
        => _weeklyParkingSpotsSpotRepository.GetAll().SelectMany(x => x.Reservations)
            .Select(x => new ReservationDto
            {
                Id = x.Id,
                ParkingSpotId = x.ParkingSpotId,
                EmployeeName = x.EmployeeName,
                Date = x.Date.Value.Date
            });

    public Guid? Create(CreateReservation command)
    {
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var weeklyParkingSpot = _weeklyParkingSpotsSpotRepository.Get(parkingSpotId);
        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(command.ReservationId, command.ParkingSpotId, command.EmployeeName,
            command.LicensePlate, command.Date);
        weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));
        _weeklyParkingSpotsSpotRepository.Update(weeklyParkingSpot);
        
        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlate command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
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
        _weeklyParkingSpotsSpotRepository.Update(weeklyParkingSpot);
        return true;
    }

    public bool Delete(DeleteReservation command)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(command.ReservationId);
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
        _weeklyParkingSpotsSpotRepository.Delete(weeklyParkingSpot);
        return true;
    }

    private WeeklyParkingSpot GetWeeklyParkingSpotByReservation(Guid reservationId)
        => _weeklyParkingSpotsSpotRepository.GetAll()
            .SingleOrDefault(x => x.Reservations.Any(r => r.Id == reservationId));
}
