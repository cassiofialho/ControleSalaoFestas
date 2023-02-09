using FinalProject.Models.Domains;
using FinalProject.Models.DTO;
using FinalProject.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Repositories.Implementation
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly DatabaseContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserAuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, DatabaseContext ctx)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _ctx = ctx;
        }

        public async Task<Status> Login(LoginModel login)
        {
            var status = new Status();

            var user = await _userManager.FindByEmailAsync(login.Email);
            if(user == null)
            {
                status.StatusCode = 0;
                status.Mensagem = "Usuário não existe.";
                status.Role = "";
                return status;
            }

            if(!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                status.StatusCode = 0;
                status.Mensagem = "Senha incorreta.";
                status.Role = "";
                return status;
            }

            var signInUser = await _signInManager.PasswordSignInAsync(user, login.Password, true, true);

            if (signInUser.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    status.Role = userRole;
                }
                status.StatusCode = 1;
                status.Mensagem = "Bem vindo!";
            }
            else if (signInUser.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Mensagem = "Usuário bloqueado!";
                status.Role = "";
            }
            else
            {
                status.StatusCode = 0;
                status.Mensagem = "Erro ao tentar logar, entre em contato com o suporte!";
                status.Role = "";
            }

            return status;
        }

        public async Task<Status> Register(Morador morador)
        {
            var status = new Status();
            var userExists = await _userManager.FindByEmailAsync(morador.Email);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Mensagem = "Usuário já existe!";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = morador.Email,
                SecurityStamp = morador.MoradorId.ToString(),
                UserName = morador.Nome.Split(" ")[0],
                Name = morador.Nome,
                PhoneNumber = morador.Telefone,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user, morador.Senha);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Mensagem = "Erro ao tentar cadastrar, entre em contato com o suporte!";
                return status;
            }

            if (!await _roleManager.RoleExistsAsync(morador.Acesso))
                await _roleManager.CreateAsync(new IdentityRole(morador.Acesso));


           if (await _roleManager.RoleExistsAsync(morador.Acesso))
            {
                await _userManager.AddToRoleAsync(user, morador.Acesso);
            }

            _ctx.Morador.Add(morador);
            _ctx.SaveChanges();
            status.StatusCode = 1;
            status.Mensagem = "Cadastro realizado com sucesso!";
            return status;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Status> ForgotPassword(ChangePasswordModel changePassword)
        {
            throw new NotImplementedException();
        }
    }
}
