using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lottery.Db
{
    public class Line
    {
        [NotMapped]
        [IgnoreDataMemberAttribute]
        public const int Size = 3;

        [NotMapped]
        [IgnoreDataMemberAttribute]
        private readonly int[] allowed = { 0, 1, 2 };

        [Required]
        public string Data { get; }

        [Key]
        [IgnoreDataMemberAttribute]
        public int Id { get; set; }

        [IgnoreDataMemberAttribute]
        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        [IgnoreDataMemberAttribute]
        public Ticket ParentTicket { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMemberAttribute]
        public DateTime Inserted { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [IgnoreDataMemberAttribute]
        public DateTime Updated { get; set; }

        [Required]
        [IgnoreDataMemberAttribute]
        public int Result { get; }

        private Line()
        {
        }

        private int GetResult(IList<int> numbers)
        {
            if (numbers.Sum() == 2)
            {
                return 10;
            }

            if (numbers.All(n => n == numbers.First()))
            {
                return 5;
            }

            if (numbers[1] != numbers.First() && numbers[2] != numbers.First())
            {
                return 1;
            }

            return 0;
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

            Result = GetResult(numbers);
            Data = JsonConvert.SerializeObject(numbers);
        }
    }
}