using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lottery;
using Lottery.Models;

namespace Test
{
    [TestClass]
    public class ApiTest
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ApiTest()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
        }

        [TestCleanup]
        public void TearDown()
        {
            using (var context = new TicketContext(new DbContextOptionsBuilder<TicketContext>()
                .UseInMemoryDatabase(CustomWebApplicationFactory<Startup>.TestDbName).Options))
            {
                foreach (var ticket in context.Tickets)
                {
                    context.Tickets.Remove(ticket);
                }
                foreach (var line in context.Lines)
                {
                    context.Lines.Remove(line);
                }

                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task GetAllTicketsEmpty() =>
            Assert.AreEqual("[]", await _factory.CreateClient().GetStringAsync("ticket").ConfigureAwait(false));


        [TestMethod]
        public async Task GetTicketNotFound() =>
            Assert.AreEqual(HttpStatusCode.NotFound, (await _factory.CreateClient().GetAsync("ticket/1").
            ConfigureAwait(false)).StatusCode);

        [TestMethod]
        public async Task PutTicketNotFound() =>
            Assert.AreEqual(HttpStatusCode.NotFound, (await _factory.CreateClient().PutAsJsonAsync("ticket/1",
            new StringContent(string.Empty, Encoding.Default, "application/json")).ConfigureAwait(false))
            .StatusCode);

        [TestMethod]
        public async Task CreateTicket()
        {
            var response = await _factory.CreateClient().PostAsJsonAsync("ticket",
                new StringContent("[[2,2,2],[1,1,1]]",
                Encoding.Default, "application/json")).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
        }
    }
}
