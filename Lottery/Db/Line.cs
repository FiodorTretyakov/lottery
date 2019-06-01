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
        public const int[] Allowed = [0, 1, 2];

        private int ticketId;

        [Key]
        public int Id { get; set; }

        [ForeignKey("ticketId")]
        public Ticket ParentTicket { get; set; }

        public ICollection<int> Numbers { get; private set; }

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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; }

        public Line(int[] numbers)
        {
            if (numbers.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"Line length should be {Size}, but {numbers.Length}");
            }

            if (numbers.Any(n => !Allowed.Any(a => a == n)))
            {
                  throw new ArgumentOutOfRangeException($"There are only allowed values {string.Join(","Allowed)}, but {string.Join(",", numbers)}");              
            }

            Numbers = numbers;
        }
    }
}