using FinalProject.Models.Domains;
using FinalProject.Models.DTO;

namespace FinalProject.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> Login(LoginModel login);
        Task<Status> Register(Morador register);
        Task Logout();
        Task<Status> ForgotPassword(ChangePasswordModel changePassword);
    }
}
