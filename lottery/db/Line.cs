using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lottery.db
{
    public class Line
    {
        [NotMapped]
        public const int Size = 3;

        [Key]
        public int Id { get; set; }

        [ForeignKey("TicketId")]
        public Ticket ParentTicket { get; set; }

        public int TicketId;

        public int[] Numbers { get; private set; }

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