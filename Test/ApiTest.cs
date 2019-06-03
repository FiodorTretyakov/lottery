using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Lottery.Models;

namespace Test
{
    [TestClass]
    public sealed class ApiTest
    {
        private static TestServerFixture fixture;

        private static Uri GetUri(string route) => new Uri($"{fixture.Client.BaseAddress}{route}");

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Debug.WriteLine($"Server started with test: {context.TestName}");
            fixture = new TestServerFixture();
        }

        [ClassCleanup]
        public static void Finish() => fixture.Dispose();

        [TestCleanup]
        public void TearDown()
        {
            using (var context = new TicketContext(new DbContextOptionsBuilder<TicketContext>()
                .UseInMemoryDatabase(TestServerFixture.TestDbName).Options))
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
            Assert.AreEqual("[]", await fixture.Client.GetStringAsync(GetUri("ticket")).ConfigureAwait(false));

        [TestMethod]
        public async Task CreateTicket()
        {
            var response = await fixture.Client.PostAsJsonAsync(GetUri("ticket"),
                new StringContent("[[2,2,2],[1,1,1]]",
                Encoding.UTF8, "application/json")).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
        }
    }
}
