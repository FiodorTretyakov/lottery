using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lottery.db
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ICollection<Line> Lines { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; }
    }
}