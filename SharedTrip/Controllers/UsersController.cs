using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Common;
using SharedTrip.Contracts;
using SharedTrip.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private IValidator validator;
        private IUserService userService;
        public UsersController(IValidator validator
            ,IUserService userService)
        {
            this.validator = validator;
            this.userService = userService;
        }

        public HttpResponse Login()
            => View();

        [HttpPost]
        public HttpResponse Login(UserLogInFormModel model)
        {
            if (!userService.UserExists(model))
                return Error("Wrong username or password.");

            SignIn(userService.GetUserId(model));

            return Redirect("/Trips/All");
        }
        public HttpResponse Register()
            => View();

        [HttpPost]
        public HttpResponse Register(UserRegisterFormModel model)
        {
            List<string> errors = validator.UserRegistrationValidate(model).ToList();

            if (userService.UsernameExists(model.Username))
                errors.Add("User with the same username already exists.");

            if (userService.EmailExists(model.Email))
                errors.Add("User with the same email already exists.");

            if(errors.Count > 0)
                return Error(errors);

            userService.RegisterUser(model);

            return Redirect("/Users/Login");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            SignOut();

            return Redirect("/");
        }
    }
}
