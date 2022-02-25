using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Common;
using SharedTrip.Contracts;
using SharedTrip.Models.Trips;
using System.Collections.Generic;
using System.Linq;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private ITripService tripService;
        private IValidator validator;

        public TripsController(ITripService tripService,
            IValidator validator)
        {
            this.tripService = tripService;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse Add()
            => View();


        [HttpPost]
        [Authorize]
        public HttpResponse Add(TripCreaterFormModel model)
        {
            List<string> errors = validator.TripCreationValidate(model).ToList();

            if (errors.Count > 0)
                return Error(errors);

            tripService.CreateTrip(model, User.Id);

            return Redirect("/Trips/All");
        }


        [Authorize]
        public HttpResponse All()
            => View(tripService.GetAllTrips().ToList());
        

        [Authorize]
        public HttpResponse Details(string tripId)
            => View(tripService.GetTripDetails(tripId));

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!tripService.AreSeatsAvailable(tripId))
                return Error("There are no more free seats left.");

            if (tripService.TripHasUser(tripId, User.Id))
                return Error("You are already enlisted for this trip.");

            tripService.AddUserToTrip(tripId, User.Id);

            return Redirect("/");
        }

    }
}
