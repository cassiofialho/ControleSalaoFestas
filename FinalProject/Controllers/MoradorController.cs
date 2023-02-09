using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using FinalProject.Models.Domains;
using FinalProject.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "Morador")]
    public class MoradorController : Controller
    {
        private readonly IMoradorService morador;
        private readonly IReservasService reservas;
        private INotyfService notifyService { get; }
        public MoradorController(IMoradorService _morador, IReservasService _reservas, INotyfService _notifyService)
        {
            morador = _morador;
            reservas = _reservas;
            notifyService = _notifyService;
        }
        public IActionResult Configuracoes()
        {
            return View();
        }
        public IActionResult Suporte()
        {
            return View();
        }
        public IActionResult Conta()
        {
            var email = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(o => o.Value).FirstOrDefault();
            var conta = morador.ListarInfos(email);
            return View(conta);
        }
        public IActionResult Reservas()
        {
            var email = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(o => o.Value).FirstOrDefault();
            var reserva = reservas.ListarInfos(email);
            ViewData["numeroApartamento"] = morador.PegaNumeroApartamento(email);
            return View(reserva);
        }

        [HttpPost]
        public IActionResult AlterarReserva(Reserva reserva)
        {
            var result = reservas.AlterarDados(reserva);

            if(result.StatusCode == 1)
            {
                notifyService.Success(result.Mensagem);
                return RedirectToAction("Reservas", "Morador");
            }
            else
            {
                notifyService.Error(result.Mensagem);
                return RedirectToAction("Reservas", "Morador");
            }
        }

        [HttpPost]
        public IActionResult CadastrarReserva(Reserva reserva)
        {
            var email = User.Claims.Where(x => x.Type == ClaimTypes.Email).Select(o => o.Value).FirstOrDefault();
            var result = reservas.CadastrarDados(reserva, email);

            if (result.StatusCode == 1)
            {
                notifyService.Success(result.Mensagem);
                return RedirectToAction("Reservas", "Morador");
            }
            else
            {
                notifyService.Error(result.Mensagem);
                return RedirectToAction("Reservas", "Morador");
            }
        }

        [HttpPost]
        public IActionResult ApagarReserva(int Id)
        {
            var result = reservas.ApagarDados(Id);

            if (result.StatusCode == 1)
            {
                notifyService.Success(result.Mensagem);
                return RedirectToAction("Reservas", "Morador");
            }
            else
            {
                notifyService.Error(result.Mensagem);
                return RedirectToAction("Reservas", "Morador");
            }
        }
    }
}
