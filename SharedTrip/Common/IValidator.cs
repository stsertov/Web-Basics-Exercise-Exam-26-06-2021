using SharedTrip.Models.Trips;
using SharedTrip.Models.Users;
using System.Collections.Generic;

namespace SharedTrip.Common
{
    public interface IValidator
    {
        IEnumerable<string> UserRegistrationValidate(UserRegisterFormModel model);

        IEnumerable<string> TripCreationValidate(TripCreaterFormModel model);

    }
}
