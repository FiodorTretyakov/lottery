using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Lottery.Db
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [IgnoreDataMemberAttribute]
        private ICollection<Line> lines;

        [NotMapped]
        public ICollection<Line> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                if (IsChecked)
                {
                    throw new FieldAccessException($"Ticket is checked now and can't be updated.");
                }

                if (value?.Count == 0)
                {
                    throw new ArgumentOutOfRangeException($"Ticket should have at least 1 line");
                }

                lines = value;
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
        public Ticket(ICollection<Line> linesData)
        {
            Lines = linesData;
        }
    }
}