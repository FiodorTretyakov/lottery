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

        private static Uri GetUri(string route)
        {
            return new Uri($"{fixture.Client.BaseAddress}{route}");
        }

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            Debug.WriteLine($"Server started with test: {context.TestName}");
            fixture = new TestServerFixture();
        }

        [ClassCleanup]
        public static void Finish()
        {
            fixture.Dispose();
        }

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
        public async Task GetAllTicketsEmpty()
        {
            var response = await fixture.Client.GetAsync(GetUri("ticket")).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            Assert.AreEqual("[]", await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }

        [TestMethod]
        public async Task CreateTicket()
        {
            var response = await fixture.Client.PostAsync(GetUri("ticket"),
                new StringContent(JsonConvert.SerializeObject(new List<int[]> { new int[] { 1, 1, 1 } }),
                Encoding.UTF8, "application/json")).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
        }
    }
}
