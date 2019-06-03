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
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public List<Line> Lines { get; } = new List<Line>();

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (value)
                {
                    isChecked = value;
                }
                else
                {
                    throw new InvalidOperationException($"Ticket that checked, can't be unchecked again.");
                }
            }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Inserted { get; } = DateTime.UtcNow;

        private Ticket()
        {
        }

        public Ticket(string data)
        {
            Lines.AddRange(DeserializeLines(data));
        }

        public static List<Line> DeserializeLines(string data)
        {
            var lines = JsonConvert.DeserializeObject<ICollection<int[]>>(data).Select(lineData =>
                new Line(lineData)).ToList();

            if (lines.Count == 0)
            {
                throw new ArgumentOutOfRangeException($"The ticket should have more than {lines.Count} lines.");
            }

            return lines;
        }
    }
}