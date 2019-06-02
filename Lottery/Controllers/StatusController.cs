using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lottery.Db;

namespace Lottery.Controllers
{
    [Route("status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpPut("/{id}")]
        public static async Task Put(int id)
        {
            using (var context = new TicketContext())
            {
                var ticket = await context.Tickets.FindAsync(id).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}