using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Domains
{
    public class Morador
    {
        [Key]
        public int MoradorId { get; set; }

        //public Guid MoradorId = Guid.NewGuid();

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RG { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        [Compare("Senha")]
        public string ConfirmarSenha { get; set; }

        [Required]
        public int numeroApartamento { get; set; }
        public string Acesso { get; set; }
        
    }
}
