using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Lottery.Db
{
    public class Line
    {
        [NotMapped]
        public const int Size = 3;

        [NotMapped]
        private readonly int[] allowed = { 0, 1, 2 };

        [Required]
        public IList<int> Numbers { get; }

        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket ParentTicket { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated { get; set; }

        public int Result
        {
            get
            {
                if (Numbers.Sum() == 2)
                {
                    return 10;
                }

                if (Numbers.All(n => n == Numbers.First()))
                {
                    return 5;
                }

                if (Numbers[1] != Numbers.First() && Numbers[2] != Numbers.First())
                {
                    return 1;
                }

                return 0;
            }
        }

        public Line(IList<int> numbers)
        {
            if (numbers.Count != Size)
            {
                throw new ArgumentOutOfRangeException($"Line length should be {Size}, but {numbers.Count}");
            }

            if (numbers.Any(n => !allowed.Any(a => a == n)))
            {
                throw new ArgumentOutOfRangeException($"There are only allowed values {string.Join(",", allowed)}, but {string.Join(",", numbers)}");
            }

            Numbers = numbers;
        }
    }
}