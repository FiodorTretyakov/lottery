using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lottery.Db
{
    public class Ticket
    {
        [Key]
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

        [IgnoreDataMemberAttribute]
        private bool isChecked;

        [NotMapped]
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
        public DateTime Inserted { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [IgnoreDataMemberAttribute]
        public DateTime Updated { get; set; }

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
            JsonConvert.DeserializeObject<ICollection<IList<int>>>(data).Select(lineData =>
            new Line(lineData)).ToList();
    }
}