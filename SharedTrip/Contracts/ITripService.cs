using SharedTrip.Models.Trips;
using System.Collections.Generic;

namespace SharedTrip.Contracts
{
    public interface ITripService
    {
        void CreateTrip(TripCreaterFormModel model, string userId);

        void AddUserToTrip(string tripId, string userId);

        bool AreSeatsAvailable(string tripId);

        bool TripHasUser(string tripId, string userId);
        IEnumerable<TripExportViewModel> GetAllTrips();

        TripDetailsViewModel GetTripDetails(string tripId);

    }
}
