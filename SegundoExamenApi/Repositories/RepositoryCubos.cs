using Microsoft.EntityFrameworkCore;
using SegundoExamenApi.Data;
using SegundoExamenApi.Models;

namespace SegundoExamenApi.Repositories
{
    public class RepositoryCubos
    {

        private ExamenContext context;

        public RepositoryCubos(ExamenContext context)
        { 
            this.context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        public async Task<Cubo> FindCuboAsync(int idCubo)
        {
            return await this.context.Cubos.FirstOrDefaultAsync(x => idCubo == idCubo);
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        { 
            return await this.context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> LoginUsuarioAsync(string email, string password)
        {
            return await this.context.Usuarios.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task<Usuario> FindUsuario(int idUsuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => idUsuario == idUsuario);
        }


        public async Task<List<Cubo>> BuscarCubosPorMarcaAsync(string marca)
        {
            return await this.context.Cubos.Where(c => c.Marca == marca).ToListAsync();
        }


        public async Task<List<Pedido>> VerPedidosDelUsuarioAsync(int idUsuario)
        {
            return await this.context.Pedidos.Where(p => p.IdUsuario == idUsuario).ToListAsync();
        }


        public async Task<List<Usuario>> GetUsuarioAsync(int idusuario)
        {
            return await this.context.Usuarios.Where(x => x.IdUsuario == idusuario).ToListAsync();
        }

        public async Task<List<Pedido>> GetPedidosUsuarioAsync(int idusuario)
        {
            return await this.context.Pedidos.Where(x => x.IdUsuario == idusuario).ToListAsync();
        }



        public async Task InsertarUsuarioAsync(Usuario nuevoUsuario)
        {
            this.context.Usuarios.Add(nuevoUsuario);
            await this.context.SaveChangesAsync();
        }



    }
}
