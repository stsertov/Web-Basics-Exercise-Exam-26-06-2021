using SharedTrip.Models.Users;

namespace SharedTrip.Contracts
{
    public interface IUserService
    {
        void RegisterUser(UserRegisterFormModel model);
        string GetUserId(UserLogInFormModel model);
        bool UserExists(UserLogInFormModel model);
        bool UsernameExists(string username);
        bool EmailExists(string email);
    }
}
