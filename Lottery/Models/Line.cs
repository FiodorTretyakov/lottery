using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Lottery.Models
{
    public class Line
    {
        [NotMapped]
        public const int Size = 3;

        [NotMapped]
        private readonly int[] allowed = { 0, 1, 2 };

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        private string numbers;

        public IList<int> Numbers
        {
            get
            {
                return JsonConvert.DeserializeObject<IList<int>>(numbers);
            }
        }

        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket ParentTicket { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public DateTime Inserted { get; } = DateTime.UtcNow;

        [NotMapped]
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

        private Line()
        {
        }

        public Line(int[] nums)
        {
            if (nums.Length != Size)
            {
                throw new ArgumentOutOfRangeException($"Line length should be {Size}, but {nums.Length}");
            }

            if (nums.Any(n => !allowed.Any(a => a == n)))
            {
                throw new ArgumentOutOfRangeException($"There are only allowed values {string.Join(",", allowed)}, but {string.Join(",", nums)}");
            }

            numbers = JsonConvert.SerializeObject(nums);
        }
    }
}