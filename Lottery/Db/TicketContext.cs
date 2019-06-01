using Microsoft.EntityFrameworkCore;

namespace Lottery.Db
{
    public class TicketContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Line> Lines { get; set; }
    }
}