using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.Repositories.Interface;
using TicketSystem.ViewModel;

namespace TicketSystem.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<Role> roleManager;

        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager,
                IConfiguration configuration, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<string> SignIn(SignInVM entity)
        {
            var user = await userManager.FindByEmailAsync(entity.Email??"");
            if (user == null) { return "Email Not InValid"; }
            var password = await userManager.CheckPasswordAsync(user, entity.Password ?? "");
           
            if ( !password)
            {
                return "PassWord not InValid";
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()), 
                new Claim(ClaimTypes.Email, entity.Email ?? ""),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey,
                    SecurityAlgorithms.HmacSha512Signature)
                    );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
