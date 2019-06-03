using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Lottery.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [NotMapped]
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
        public Ticket(IList<Line> linesData)
        {
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