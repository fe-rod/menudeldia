using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuDelDia.Entities
{
    [Table("OpenDays")]
    public class OpenDay:EntityBase
    {
        [Key]
        public override Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public int OpenHour { get; set; }
        public int OpenMinutes { get; set; }
        public int CloseHour { get; set; }
        public int CloseMinutes { get; set; }

    }
}
