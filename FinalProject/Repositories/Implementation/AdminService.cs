using FinalProject.Models.Domains;
using FinalProject.Models.DTO;
using FinalProject.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Net.Mime;

namespace FinalProject.Repositories.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly DatabaseContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, DatabaseContext ctx)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _ctx = ctx;
        }

        public Administrador ListarInfo(string email)
        {
            var admin = _ctx.Administrador.Where(x => x.Email == email).FirstOrDefault();

            return admin;
        }

        public List<Reserva> ListarReservas()
        {
            var reservas = _ctx.Reserva.ToList();

            foreach(var reserva in reservas)
            {
                var moradorInfos = _ctx.Morador.Where(x => x.MoradorId == reserva.MoradorId).ToList();
                
                foreach(var morador in moradorInfos)
                {
                    reserva.Morador = morador;
                }
            }
            return reservas;
        }

        public async Task<Status> RegisterAdmin(Administrador admin)
        {
            var status = new Status();
            var userExists = await _userManager.FindByEmailAsync(admin.Email);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Mensagem = "Usuário já existe!";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = admin.Email,
                SecurityStamp = admin.AdminId.ToString(),
                UserName = admin.Nome.Split(" ")[0],
                Name = admin.Nome,
                PhoneNumber = admin.Telefone,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await _userManager.CreateAsync(user, admin.Senha);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Mensagem = "Erro ao tentar cadastrar, entre em contato com o suporte!";
                return status;
            }

            if (!await _roleManager.RoleExistsAsync(admin.Role))
                await _roleManager.CreateAsync(new IdentityRole(admin.Role));


            if (await _roleManager.RoleExistsAsync(admin.Role))
            {
                await _userManager.AddToRoleAsync(user, admin.Role);
            }

            _ctx.Administrador.Add(admin);
            _ctx.SaveChanges();

            //EnviarEmail(admin.Email, $"<h3>Email para acessar o sistema: {admin.Email}</h3> - <h3>Senha para acessar o sistema: {admin.Senha}</h3>", "Credenciais para acessar o sistema");

            status.StatusCode = 1;
            status.Mensagem = "Cadastro realizado com sucesso!";
            return status;
        }
        private string EnviarEmail(string email, string mensagem, string assunto)
        {
            try
            {
                //Passando Remetente e destinatário respectivamente
                MailMessage mail = new MailMessage("controlesalaofestas@gmail.com", email);

                //Assunto
                mail.Subject = assunto;
                //Body é um objeto HTML
                mail.IsBodyHtml = true;
                //Corpo
                mail.Body = mensagem;
                mail.BodyEncoding = Encoding.GetEncoding("UTF-8");
                mail.SubjectEncoding = Encoding.GetEncoding("UTF-8");

                //Passando Host e Porta SMTP
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 465);
                smtp.UseDefaultCredentials = false;
                //Passando as credenciais do Email Remetente(Email e Senha respectivamente)
                smtp.Credentials = new NetworkCredential("controlesalaofestas@gmail.com", "controleSalao@1234");
                smtp.EnableSsl = false;
                smtp.Send(mail);

                //Necessário Tratar erros
                return "";
            }
            catch
            {
                return "";
            }
        }

    }
}
