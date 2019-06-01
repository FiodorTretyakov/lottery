using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lottery.Db
{
    public class Line
    {
        [NotMapped]
        public const int Size = 3;

        [Key]
        public int Id { get; set; }

        [ForeignKey("ticketId")]
        public Ticket ParentTicket { get; set; }

        private int ticketId;

        public ICollection<int> Numbers { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; set; }

        public Line(int[] numbers)
        {
            if (numbers.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"Line length should be {Size}, but {numbers.Length}");
            }

            Numbers = numbers;
        }
    }
}