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
        private readonly IList<Line> lines = new List<Line>();

        [NotMapped]
        [IgnoreDataMemberAttribute]
        public ICollection<Line> Lines
        {
            get
            {
                return IsChecked ? new ReadOnlyCollection<Line>(lines) : lines;
            }
        }

        private bool isChecked;

        [NotMapped]
        [IgnoreDataMemberAttribute]
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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [IgnoreDataMemberAttribute]
        public DateTime Updated { get; set; }

        private Ticket()
        {
        }
        public Ticket(IList<Line> linesData)
        {
            if (linesData.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Ticket should be created at least with one line.");
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