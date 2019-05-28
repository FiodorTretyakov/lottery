using System.ComponentModel.DataAnnotations;

namespace lottery.db
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Data { get; set; }
    }
}