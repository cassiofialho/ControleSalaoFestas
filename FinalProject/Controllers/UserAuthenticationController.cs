using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using FinalProject.Models.Domains;
using FinalProject.Models.DTO;
using FinalProject.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace FinalProject.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService auth;
        private INotyfService notifyService { get; }
        public UserAuthenticationController(IUserAuthenticationService _auth, INotyfService _notifyService)
        {
            auth = _auth;
            notifyService = _notifyService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var result = await auth.Login(login);

            if(result.StatusCode == 1)
            {
                if(result.Role == "Morador")
                {
                    notifyService.Success(result.Mensagem);
                    return RedirectToAction("Conta", "Morador");
                }
                else
                {
                    notifyService.Success(result.Mensagem);
                    return RedirectToAction("Conta", "Administrador");
                }
            }
            else
            {
                notifyService.Error(result.Mensagem);
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Morador morador)
        {
            if (!ModelState.IsValid)
            {
                return View(morador);
            }
            var result = await auth.Register(morador);

            if(result.StatusCode == 1)
            {
                return RedirectToAction("Login", "UserAuthentication");
            }
            else
            {
                TempData["msg"] = result.Mensagem;
                return RedirectToAction(nameof(Register));
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ChangePasswordModel password)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await auth.Logout();
            return RedirectToAction("Login", "UserAuthentication");
        }
    }
}
