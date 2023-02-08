﻿using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();

    public Guid Id { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public string Name { get; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public WeeklyParkingSpot(Guid id, DateTime from, DateTime to, string name)
    {
        Id = id;
        From = from;
        To = to;
        Name = name;
    }

    public void AddReservation(Reservation reservation)
    {
        var isInvalidDate = reservation.Date.Date < From || 
                            reservation.Date.Date > To ||
                            reservation.Date.Date < DateTime.UtcNow.Date;
        if (isInvalidDate)
        {
            throw new InvalidReservationDateException(reservation.Date);
        }
        
        var reservationAlreadyExists = Reservations.Any(x =>
            x.Date.Date == reservation.Date.Date);
        if (reservationAlreadyExists)
        {
            throw new ParkingSpotNameAlreadyReservedException(Name, reservation.Date);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(Guid id)
        => _reservations.RemoveWhere(x => x.Id == id);
}
