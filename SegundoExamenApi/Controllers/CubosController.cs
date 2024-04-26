using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SegundoExamenApi.Models;
using SegundoExamenApi.Repositories;
using System.Numerics;
using System.Security.Claims;

namespace SegundoExamenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {

        private RepositoryCubos repo;
        private IHttpContextAccessor httpContextAccessor;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpGet("cubos")]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubosAsync();
        }

        [HttpGet("cubos/marca/{marca}")]
        public async Task<ActionResult<List<Cubo>>> BuscarCubosPorMarca(string marca)
        {
            try
            {
                var cubos = await repo.BuscarCubosPorMarcaAsync(marca);
                if (cubos.Count == 0)
                    return NotFound("No se encontraron cubos con la marca especificada.");

                return Ok(cubos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al buscar cubos por marca: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cubo>> FindCubo(int id)
        {
            Cubo cubo = await this.repo.FindCuboAsync(id);

            if (cubo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cubo);
            }

        }

        //[Authorize]
        [HttpGet("usuarios")]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            return await this.repo.GetUsuariosAsync();
        }


        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> FindUsuario()
        {
            Claim claim = HttpContext.User.FindFirst(x => x.Type == "UserData");
            string jsonUser = claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUser);
            return usuario;
        }


        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Pedido>>> Pedidos()
        {
            string jsonUser = HttpContext.User.FindFirst(x => x.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUser);
            List<Pedido> pedidos = await this.repo.GetPedidosUsuarioAsync(usuario.IdUsuario);
            return pedidos;
        }





    }
}
