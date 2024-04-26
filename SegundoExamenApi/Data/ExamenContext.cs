using Microsoft.EntityFrameworkCore;
using SegundoExamenApi.Models;
using System.Numerics;

namespace SegundoExamenApi.Data
{
    public class ExamenContext:DbContext
    {
        public ExamenContext(DbContextOptions<ExamenContext> options) : base(options) { }
        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

    }
}
