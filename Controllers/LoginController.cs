using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Contexts;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {


        private readonly TeleloaderSqlSever context;
        private readonly IConfiguration _configuration;
        public LoginController(TeleloaderSqlSever context, IConfiguration configuration)
        {
            this.context = context;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("session")]
        public dynamic initSession([FromBody] Object login)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(login.ToString());

            string usuario = data.usuario;
            string password = data.password;

            var user = context.usuario.Where(x => x.Login == usuario && x.password == password).FirstOrDefault();


            if (user == null)
            {
                return new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    result = ""

                };
            }

            var jwt = _configuration.GetSection("jwt").Get<Jwt>();

            var claims = new[] {
               new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
               new Claim("id",user.id.ToString()),
               new Claim("usuario", user.Login)
               
               
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(4),
                    signingCredentials: singIn
                );

            return new
            {
                success = true,
                message = "Exitoso",
                result = new JwtSecurityTokenHandler().WriteToken(token)

            };



        }


    }
}
