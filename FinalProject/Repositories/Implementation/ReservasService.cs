using FinalProject.Models.Domains;
using FinalProject.Models.DTO;
using FinalProject.Repositories.Abstract;
using System.Security.Claims;

namespace FinalProject.Repositories.Implementation
{
    public class ReservasService : IReservasService
    {
        private readonly DatabaseContext ctx;

        public ReservasService(DatabaseContext _ctx)
        {
            ctx = _ctx;
        }

        public Reserva ListarInfos(string email)
        {
            var usuario = ctx.Morador.Where(u => u.Email == email).FirstOrDefault();

            var reserva = ctx.Reserva.Where(x => x.MoradorId == usuario.MoradorId).FirstOrDefault();

            if(reserva == null)
            {
                reserva = new();
            }

            return reserva;
        }

        public Reserva ListarPorId(int Id)
        {
            //Busca contato no banco de acordo com seu Id
            var reserva = ctx.Reserva.Where(x => x.ReservaId == Id).FirstOrDefault();

            return reserva;
        }

        public Status AlterarDados(Reserva reserva)
        {
            var status = new Status();
            var verificaReserva = ctx.Reserva.Where(x => x.DtReserva == reserva.DtReserva).ToList();

            if (verificaReserva.Count == 0)
            {
                Reserva reservaDb = ListarPorId(reserva.ReservaId);
                if (reservaDb == null)
                {
                    throw new Exception("Houve um erro de atualização");
                }

                reservaDb.DtReserva = reserva.DtReserva;
                reservaDb.MoradorId = reserva.MoradorId;

                ctx.Reserva.Update(reservaDb);
                ctx.SaveChanges();

                status.StatusCode = 1;
                status.Mensagem = "Reserva realizada para o dia: " + reservaDb.DtReserva.ToString("dd/MM/yyyy");
                return status;
            }
            else
            {
                status.StatusCode = 0;
                status.Mensagem = "Já possui uma reserva para essa data!";
                return status;
            }
        }

        public Status CadastrarDados(Reserva reserva, string email)
        {
            var status = new Status();
            var usuarioId = ctx.Morador.Where(x => x.Email == email).Select(o => o.MoradorId).FirstOrDefault();
            var verificaReserva = ctx.Reserva.Where(x => x.DtReserva == reserva.DtReserva).ToList();

            if (verificaReserva.Count == 0)
            {
                try
                {
                    reserva.MoradorId = usuarioId;
                    ctx.Reserva.Add(reserva);
                    ctx.SaveChanges();

                    status.StatusCode = 1;
                    status.Mensagem = "Reserva realizada para o dia: " + reserva.DtReserva.ToString("dd/MM/yyyy");
                    return status;
                }
                catch (Exception e)
                {
                    status.StatusCode = 1;
                    status.Mensagem = e.ToString();
                    return status;
                }
            }
            else
            {
                status.StatusCode = 0;
                status.Mensagem = "Já possui uma reserva para essa data!";
                return status;
            }
        }

        public Status ApagarDados(int Id)
        {
            var status = new Status();
            var reserva = ListarPorId(Id);

            try
            {
                ctx.Reserva.Remove(reserva);
                ctx.SaveChanges();

                status.StatusCode = 1;
                status.Mensagem = "Reserva apagada!";
                return status;
            }
            catch (Exception e)
            {
                status.StatusCode = 0;
                status.Mensagem = e.ToString();
                return status;
            }
        }
    }
}
