using FinalProject.Models.Domains;

namespace FinalProject.Repositories.Abstract
{
    public interface IMoradorService
    {
        int PegaNumeroApartamento(string email);
        Morador ListarInfos(string email);
    }
}
