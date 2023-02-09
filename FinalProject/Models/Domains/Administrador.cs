using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Domains
{
    public class Administrador
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        [Compare("Senha")]
        public string ConfirmarSenha { get; set; }

        [Required]
        public string Telefone { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
