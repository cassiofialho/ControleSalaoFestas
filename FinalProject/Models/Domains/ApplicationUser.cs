using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models.Domains
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
