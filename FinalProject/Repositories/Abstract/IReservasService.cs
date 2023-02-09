using FinalProject.Models.Domains;
using FinalProject.Models.DTO;

namespace FinalProject.Repositories.Abstract
{
    public interface IReservasService
    {
        Reserva ListarInfos(string email);
        Status CadastrarDados(Reserva reserva, string email);
        Status ApagarDados(int Id);
        Status AlterarDados(Reserva reserva);
    }
}
