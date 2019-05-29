using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lottery.db
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public List<Line> Lines { get; set; }
    }
}