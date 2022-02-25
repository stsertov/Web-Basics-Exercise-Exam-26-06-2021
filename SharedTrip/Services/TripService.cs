using SharedTrip.Contracts;
using SharedTrip.Data.Common;
using SharedTrip.Data.Models;
using SharedTrip.Models.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharedTrip.Services
{
    public class TripService : ITripService
    {
        private IRepository repository;

        public TripService(IRepository repository)
        {
            this.repository = repository;
        }

        public void CreateTrip(TripCreaterFormModel model, string userId)
        {
            Trip trip = new Trip()
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime,
                                 "dd.MM.yyyy HH:mm",
                                 CultureInfo.InvariantCulture,
                                 DateTimeStyles.None),
                Description = model.Description,
                Seats = model.Seats,
                ImagePath = model.ImagePath
            };

            repository.Add(trip);
            repository.Add(new UserTrip() { Trip = trip, UserId = userId });
            repository.SaveChanges();
        }

        public IEnumerable<TripExportViewModel> GetAllTrips()
            => repository.All<Trip>()
               .Select(t => new TripExportViewModel
               {
                   Id = t.Id,
                   StartPoint = t.StartPoint,
                   EndPoint = t.EndPoint,
                   DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                   Seats = t.Seats - repository
                            .All<UserTrip>()
                            .Where(ut => ut.TripId == t.Id)
                            .Count()
               }).ToList();

        public TripDetailsViewModel GetTripDetails(string tripId)
            => repository.All<Trip>()
               .Where(t => t.Id == tripId)
               .Select(t => new TripDetailsViewModel
               {
                   Id = t.Id,
                   StartPoint = t.StartPoint,
                   EndPoint = t.EndPoint,
                   DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                   Seats = t.Seats - repository
                            .All<UserTrip>()
                            .Where(ut => ut.TripId == t.Id)
                            .Count(),
                   Description = t.Description
               }).FirstOrDefault();

        public bool AreSeatsAvailable(string tripId)
            => repository.All<Trip>().FirstOrDefault(t => t.Id == tripId).Seats - 
                  repository.All<UserTrip>().Where(ut => ut.TripId == tripId).Count() > 0;

        public bool TripHasUser(string tripId, string userId)
            => repository.All<UserTrip>()
                         .Any(ut => ut.TripId == tripId &&
                                    ut.UserId == userId);

        public void AddUserToTrip(string tripId, string userId)
        {
            repository.Add<UserTrip>(new UserTrip()
            {
                TripId = tripId,
                UserId = userId
            });
            repository.SaveChanges();
        }
    }
}
