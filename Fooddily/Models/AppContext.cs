using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fooddily.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base("Fooddily")
        {
            
        }

        public DbSet<Comercio> Comercios { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TipoTag> TiposTag { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Zona> Zonas { get; set; }
    }
}