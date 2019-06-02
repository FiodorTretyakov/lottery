using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public sealed class ApiTest : IDisposable
    {
        private TestServerFixture fixture;

        [ClassInitialize]
        public void Init()
        {
            fixture = new TestServerFixture();
        }

        [ClassCleanup]
        public void Dispose()
        {
            fixture.Dispose();
        }

        [TestMethod]
        public async Task GetAllTickets()
        {
            var response = await fixture.Client.GetAsync("ticket").ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            Assert.AreEqual("[]", await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
