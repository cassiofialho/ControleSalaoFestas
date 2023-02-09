using AspNetCoreHero.ToastNotification.Abstractions;
using FinalProject.Models.Domains;
using FinalProject.Repositories.Abstract;
using FinalProject.Repositories.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly IAdminService adminService;
        private INotyfService notifyService { get; }
        public AdministradorController(IAdminService _adminService, INotyfService _notyfService)
        {
            adminService = _adminService;
            notifyService = _notyfService;
        }

        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Conta()
        {
            var email = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(o => o.Value).FirstOrDefault();
            var adminInfos = adminService.ListarInfo(email);
            return View(adminInfos);
        }

        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Reservas()
        {
            var reservas = adminService.ListarReservas();
            return View(reservas);
        }

        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Administracao()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult Configuracoes()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        //public async Task<IActionResult> RegisterMaster()
        //{
        //    var admin = new Administrador()
        //    {
        //        Nome = "Cassio",
        //        Email = "cassio@email.com",
        //        Senha = "Admin@1234",
        //        ConfirmarSenha = "Admin@1234",
        //        Telefone = "11962887343",
        //        Role = "Admin"
        //    };
        //    var result = await adminService.RegisterAdmin(admin);
        //    return Ok(result.Mensagem);
        //}

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(Administrador admin)
        {
            admin.Senha = alfanumericoAleatorio(8) + "#";
            admin.ConfirmarSenha = admin.Senha;

            var result = await adminService.RegisterAdmin(admin);

            if (result.StatusCode == 1)
            {                
                notifyService.Success(result.Mensagem);
                return RedirectToAction(nameof(Register));
            }
            else
            {
                notifyService.Error(result.Mensagem);
                return RedirectToAction(nameof(Register));
            }
        }

        public static string alfanumericoAleatorio(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%¨&*()";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}
