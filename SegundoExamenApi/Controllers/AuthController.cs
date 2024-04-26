using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SegundoExamenApi.Helpers;
using SegundoExamenApi.Models;
using SegundoExamenApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;

namespace SegundoExamenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private RepositoryCubos repo;
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryCubos repo, HelperActionServicesOAuth helper)
        { 
            this.repo = repo;
            this.helper = helper;
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            Usuario usuario = await this.repo.LoginUsuarioAsync(model.Email, model.Password);
            if (usuario == null)
            {
                return Unauthorized();

            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(
                    this.helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);

                //PONEMOS EL EMPLEADO EN FORMATO JSON
                string jsonUsuario = JsonConvert.SerializeObject(usuario);

                //CREAMOS UN ARRAY CON TODA LA INFORMACION QUE QUEREMOS GUARDAR EN EL TOKEN
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonUsuario)
                };


                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    notBefore: DateTime.UtcNow
                    );

                //POR ULTIMO, DEVOLVEMOS UNA RESPUESTA AFIRMATIVA 
                //CON UN OBJETO ANONIMO EN FORMATO JSON 
                return Ok(
                    new
                    {
                        response = new JwtSecurityTokenHandler().WriteToken(token)
                    });

            }

        }



    }
}
