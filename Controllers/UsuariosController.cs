using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using WebApi.Contexts;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly TeleloaderSqlSever context;

        public UsuariosController(TeleloaderSqlSever context)
        {
            this.context = context;
        }

        // GET: api/<UsuariosController>
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return context.usuario.ToList();
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{id}")]
        [Authorize]
        public dynamic Get(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;


            var rToken = Jwt.validarToken(identity, context);

            if (!rToken.success) return rToken;

            Usuario usuario = rToken.Result;

            if (usuario.perfil != "administrador")
            {

                return new
                {
                    success = false,
                    message = "No tienen permisos para consultar los usuarios ",
                    Result = ""
                };

            }

            return context.usuario.FirstOrDefault(e => e.id == id);

        }

        // POST api/<UsuariosController>
        [HttpPost]
        public void Post([FromBody] Usuario value)
        {
        }

        // PUT api/<UsuariosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuariosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
