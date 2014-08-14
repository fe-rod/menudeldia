using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fooddily.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        
        public int TipoTagId { get; set; }
        public virtual TipoTag TipoTag { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}