using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Lottery.Db
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ICollection<Line> Lines { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated { get; set; }

        private Ticket()
        {
        }
        public Ticket(ICollection<IList<int>> lines)
        {
            if (lines?.Count == 0)
            {
                throw new ArgumentOutOfRangeException($"Ticket should have at least 1 line");
            }

            Lines = lines.Select(line => new Line(line)).ToList();
        }
    }
}