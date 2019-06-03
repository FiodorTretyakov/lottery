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

        [NotMapped]
        private readonly IList<Line> lines = new List<Line>();

        [Required]
        [MinLength(1)]
        public ICollection<Line> Lines
        {
            get
            {
                return IsChecked ? new ReadOnlyCollection<Line>(lines) : lines;
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
        public Ticket(IList<Line> linesData)
        {
            if (linesData.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Ticket should have at least one line.");
            }
            lines = linesData;
        }

        public Ticket(string data) : this(DeserializeLines(data))
        {
        }

        public static IList<Line> DeserializeLines(string data) =>
            JsonConvert.DeserializeObject<ICollection<int[]>>(data).Select(lineData =>
            new Line(lineData)).ToList();
    }
}