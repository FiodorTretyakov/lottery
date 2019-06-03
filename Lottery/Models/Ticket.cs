using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lottery.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        private readonly Line[] lines;

        public IEnumerable<Line> Lines
        {
            get
            {
                return IsChecked ? Array.AsReadOnly(lines) : lines.Select(line => line);
            }
        }

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
        [IgnoreDataMemberAttribute]
        public DateTime Inserted { get; } = DateTime.UtcNow;

        private Ticket()
        {
        }
        public Ticket(IEnumerable<Line> linesData)
        {
            lines = linesData.ToArray();
        }

        public Ticket(string data) : this(DeserializeLines(data))
        {
        }

        public static IEnumerable<Line> DeserializeLines(string data) =>
            JsonConvert.DeserializeObject<ICollection<int[]>>(data).Select(lineData =>
            new Line(lineData)).ToList();
    }
}