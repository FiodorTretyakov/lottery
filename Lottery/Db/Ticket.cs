using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Lottery.Db
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ICollection<Line> Lines { get; private set; }

        public bool Checked { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMemberAttribute]
        public DateTime Inserted { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [IgnoreDataMemberAttribute]
        public DateTime Updated { get; set; }

        private Ticket()
        {
        }
        public Ticket(ICollection<IList<int>> lines)
        {
            SetLines(lines);
        }

        public void SetLines(ICollection<IList<int>> lines)
        {
            if (Checked)
            {
                throw new FieldAccessException($"Ticket is checked now and can't be updated");
            }

            if (lines?.Count == 0)
            {
                throw new ArgumentOutOfRangeException($"Ticket should have at least 1 line");
            }

            Lines = lines.Select(line => new Line(line)).ToList();
        }
    }
}