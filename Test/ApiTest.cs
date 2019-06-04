using System;
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
    public class ApiTest : IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ApiTest()
        {
            factory = new CustomWebApplicationFactory<Startup>();
        }

        private HttpClient Client => factory.CreateClient();

        private Uri GetUri(string controller) => new Uri($"{factory.ClientOptions.BaseAddress}/{controller}");

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

        public void Dispose()
        {
            factory.Dispose();
        }

        [TestMethod]
        public async Task GetAllTicketsEmpty() =>
            Assert.AreEqual("[]", await Client.GetStringAsync(GetUri("ticket")).ConfigureAwait(false));


        [TestMethod]
        public async Task GetTicketNotFound() =>
            Assert.AreEqual(HttpStatusCode.NotFound, (await Client.GetAsync(GetUri("ticket/1")).
            ConfigureAwait(false)).StatusCode);

        [TestMethod]
        public async Task PutTicketNotFound() =>
            Assert.AreEqual(HttpStatusCode.NotFound, (await Client.PutAsync(GetUri("ticket/1"),
            new StringContent("[[1, 2, 0]]", Encoding.UTF8, "application/json")).ConfigureAwait(false))
            .StatusCode);

        [TestMethod]
        public async Task CreateTicket()
        {
            var response = await Client.PostAsync(GetUri("ticket"),
                new StringContent("[[1, 1, 1]]", Encoding.UTF8, "application/json")).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
        }
    }
}
