using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SegundoExamenApi.Models;
using SegundoExamenApi.Repositories;
using System.Numerics;

namespace SegundoExamenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {

        private RepositoryCubos repo;

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
        [HttpGet("usuarios/{idUsuario}/pedidos")]
        public async Task<ActionResult<List<Pedido>>> VerPedidosDelUsuario(int idUsuario)
        {
            try
            {
                var pedidos = await repo.VerPedidosDelUsuarioAsync(idUsuario);
                if (pedidos.Count == 0)
                    return NotFound("No se encontraron pedidos para el usuario especificado.");

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al obtener pedidos del usuario: " + ex.Message);
            }
        }



        [Authorize]
        [HttpGet("{idUsuario}")]
        public async Task<ActionResult<Usuario>> FindUsuario(int idUsuario)
        {
            Usuario usuario = await this.repo.FindUsuario(idUsuario);

            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(usuario);
            }

        }






    }
}
