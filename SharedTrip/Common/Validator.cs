using SharedTrip.Models.Trips;
using SharedTrip.Models.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SharedTrip.Common
{
    using static Data.DbConstants;
    public class Validator : IValidator
    {
        private string missingError = "You should enter a {0}.";
        private string lengthError = "{0} should be between {1} and {2} characters long.";
        private string seatsCountError = "{0} must be between {1} and {2}.";

        private string emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        public IEnumerable<string> UserRegistrationValidate(UserRegisterFormModel model)
        {
            List<string> errors = new List<string>();

            if (model.Username == null)
                errors.Add(string.Format(missingError, nameof(model.Username)));


            if (model.Username.Length < UsernameMinLength ||
               model.Username.Length > UsernameMaxLength)
            {
                errors.Add(string.Format(
                    lengthError, new string[]
                    {
                        nameof(model.Username),
                        UsernameMinLength.ToString(),
                        UsernameMaxLength.ToString()
                    }));
            }

            if (model.Password == null)
                errors.Add(string.Format(missingError, nameof(model.Password)));


            if (model.Password.Length < PasswordMinLength ||
               model.Password.Length > PasswordMaxLength)
            {
                errors.Add(string.Format(
                    lengthError, new string[]
                    {
                       nameof(model.Password),
                       PasswordMinLength.ToString(),
                       PasswordMaxLength.ToString()
                    }));
            }

            if (model.Password != model.ConfirmPassword)
                errors.Add("Passwords does not match.");

            if (model.Email == null)
                errors.Add(string.Format(missingError, nameof(model.Email)));

            if (!Regex.IsMatch(model.Email, emailRegex))
                errors.Add("You should add a valid email address.");

            return errors;
        }

        public IEnumerable<string> TripCreationValidate(TripCreaterFormModel model)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.StartPoint) ||
                string.IsNullOrEmpty(model.StartPoint))
            {
                errors.Add(string.Format(missingError, "starting point"));
            }

            if (string.IsNullOrWhiteSpace(model.EndPoint) ||
                string.IsNullOrEmpty(model.EndPoint))
            {
                errors.Add(string.Format(missingError, "ending point"));
            }

            bool isRealDate = DateTime.TryParseExact(model.DepartureTime,
                "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date);

            if (!isRealDate)
                errors.Add(string.Format(missingError, "valid Date"));

            if (model.Seats < SeatsMinCount || 
                model.Seats > SeatsMaxCount)
            {
                errors.Add(string.Format(
                    seatsCountError, new string[]
                    {
                        nameof(model.Seats),
                        SeatsMinCount.ToString(),
                        SeatsMaxCount.ToString()
                    }));
            }

            if (model.ImagePath != null &&
               !Uri.IsWellFormedUriString(model.ImagePath, UriKind.Absolute))
            {
                errors.Add(string.Format(missingError, "a valid Url"));
            }

            if (model.Description == null)
                errors.Add(string.Format(missingError, nameof(model.Description)));

            if (model.Description.Length > DescriptionMaxLength)
                errors.Add($"Description should not be longer than {DescriptionMaxLength} characters.");

            return errors;
        }
    }
}
