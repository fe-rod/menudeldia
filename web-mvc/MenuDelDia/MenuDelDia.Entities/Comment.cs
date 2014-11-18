using System;
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
        public string Email { get; set; }

        public DateTime DateTimeUtc { get; set; }

        [Range(0.0,5.0)]
        public int Value { get; set; }
        
        public string Message { get; set; }


        public Guid? MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
