using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lottery;
using Lottery.Models;

namespace Test
{
    [TestClass]
    public sealed class ApiTest : IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ApiTest()
        {
            factory = new CustomWebApplicationFactory<Startup>();
        }

        private HttpClient Client => factory.CreateClient();

        private Uri GetUri(string controller) => new Uri($"{factory.ClientOptions.BaseAddress}{controller}");

        private StringContent GetBody(string value) =>
            new StringContent(value, Encoding.UTF8, "application/json");

        private string DbConnection => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().
            GetConnectionString("LotteryDatabase");

        [TestCleanup]
        public async Task TearDown()
        {
            using (var context = new TicketContext(new DbContextOptionsBuilder<TicketContext>()
                .UseSqlite(DbConnection).Options))
            {
                foreach (var ticket in context.Tickets)
                {
                    context.Tickets.Remove(ticket);
                }
                foreach (var line in context.Lines)
                {
                    context.Lines.Remove(line);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
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
            GetBody("[[1, 2, 0]]")).ConfigureAwait(false)).StatusCode);

        [TestMethod]
        public async Task CreateTicket()
        {
            var ticket1 = await Client.PostAsync(GetUri("ticket"), GetBody("[[1, 1, 1]]")).ConfigureAwait(false);
   
            ticket1.EnsureSuccessStatusCode();
            var id1 = await ticket1.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.AreEqual(id1, await Client.GetStringAsync(GetUri($"ticket/{id1}")).ConfigureAwait(false));

            var ticket2 = await Client.PostAsync(GetUri("ticket"),
                GetBody("[[1, 0, 1], [0, 0, 0]]")).ConfigureAwait(false);

            ticket2.EnsureSuccessStatusCode();
            var id2 = await ticket2.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.AreEqual(id2, await Client.GetStringAsync(GetUri($"ticket/{id2}")).ConfigureAwait(false));

            Assert.AreEqual("3", await Client.GetStringAsync(GetUri("ticket")).ConfigureAwait(false));
        }
    }
}
