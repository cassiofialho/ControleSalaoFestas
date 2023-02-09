using FinalProject.Models.Domains;
using FinalProject.Repositories.Abstract;

namespace FinalProject.Repositories.Implementation
{
    public class MoradorService : IMoradorService
    {
        private readonly DatabaseContext ctx;

        public MoradorService(DatabaseContext _ctx)
        {
            ctx = _ctx;
        }

        public Morador ListarInfos(string email)
        {
            var morador = ctx.Morador.Where(x => x.Email == email).FirstOrDefault();

            return morador;
        }

        public int PegaNumeroApartamento(string email)
        {
            Morador morador = new Morador();

            morador = ctx.Morador.Where(x => x.Email == email).FirstOrDefault();

            return morador.numeroApartamento;
        }
    }
}
