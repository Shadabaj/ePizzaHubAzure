using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class AuthService : IAuthService
    {
        IUserRepository _userRepo;
        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public bool CreateUser(User user, string Role)
        {
            return _userRepo.CreateUser(user, Role);
        }

        public UserModel ValidateUser(string Email, string Password)
        {
           return _userRepo.ValidateUser(Email, Password);
        }
    }
}
