using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lottery.Models
{
    [DataContract]
    public class Line
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string NumbersData { get; private set; }

        [DataMember]
        public IList<int> Numbers
        {
            get
            {
                return JsonConvert.DeserializeObject<IList<int>>(NumbersData);
            }
        }

        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket ParentTicket { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; } = DateTime.UtcNow;

        [DataMember]
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

        public bool ShouldSerializeResult() => ParentTicket.IsChecked;

        public bool ShouldSerializeNumbers() => !ParentTicket.IsChecked;

        private Line()
        {
        }

        public Line(IList<int> nums)
        {
            const int size = 3;
            int[] allowed = { 0, 1, 2 };

            if (nums.Count != size)
            {
                throw new ArgumentOutOfRangeException($"Line length should be {size}, but {nums.Count}.");
            }

            if (nums.Any(n => !allowed.Any(a => a == n)))
            {
                throw new ArgumentOutOfRangeException($"There are only allowed values {string.Join(",", allowed)}, but {string.Join(",", nums)}.");
            }

            NumbersData = JsonConvert.SerializeObject(nums);
        }
    }
}