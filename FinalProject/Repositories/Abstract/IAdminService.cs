using FinalProject.Models.Domains;
using FinalProject.Models.DTO;

namespace FinalProject.Repositories.Abstract
{
    public interface IAdminService
    {
        Task<Status> RegisterAdmin(Administrador admin);
        List<Reserva> ListarReservas();
        Administrador ListarInfo(string email);
    }
}
