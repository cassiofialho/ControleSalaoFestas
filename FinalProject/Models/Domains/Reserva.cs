using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Domains
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        [Required]
        public DateTime DtReserva { get; set; }


        //ForeignKey
        public int MoradorId { get; set; }
        public Morador Morador { get; set; }
    }
}
