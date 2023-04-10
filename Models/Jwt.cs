using System.Security.Claims;
using WebApi.Contexts;

namespace WebApi.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }


        public static dynamic validarToken(ClaimsIdentity identity, TeleloaderSqlSever context)
        {

            try
            {

                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar si estas enviando un token valido. ",
                        Result = ""
                    };

                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
             
                Usuario usuario;

                usuario = context.usuario.FirstOrDefault(e => e.id == Int64.Parse(id));


                return new
                {
                    success = true,
                    message = "Exito ",
                    Result = usuario
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    Result = ""
                };

            }

        }


    }



}
