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
        [IgnoreDataMemberAttribute]
        private string data;

        [NotMapped]
        public IList<int> Numbers
        {
            get
            {
                return JsonConvert.DeserializeObject<IList<int>>(data);
            }
            set
            {
                if (value.Count != Size)
                {
                    throw new ArgumentOutOfRangeException($"Line length should be {Size}, but {value.Count}");
                }

                if (value.Any(n => !allowed.Any(a => a == n)))
                {
                    throw new ArgumentOutOfRangeException($"There are only allowed values {string.Join(",", allowed)}, but {string.Join(",", value)}");
                }

                data = JsonConvert.SerializeObject(value);
            }
        }

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

        public Line(IList<int> numbers)
        {
            Numbers = numbers;
        }
    }
}