using Microsoft.EntityFrameworkCore;

namespace lottery.db
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options)
            : base(options)
        { }

        public DbSet<Ticket> Tickets { get; set; }
    }
}