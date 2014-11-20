using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuDelDia.Entities
{
    [Table("Comments")]
    public class Comment:EntityBase
    {
        public Comment()
        {

        }

        [Key]
        public override Guid Id { get; set; }

        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Fecha UTC")]
        public DateTime DateTimeUtc { get; set; }

        [Range(0.0,5.0)]
        [DisplayName("Valor")]
        public int Value { get; set; }

        [DisplayName("Mensaje")]
        public string Message { get; set; }


        public Guid? MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
