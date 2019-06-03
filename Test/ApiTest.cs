using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public sealed class ApiTest
    {
        private static TestServerFixture fixture;

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

        [TestMethod]
        public async Task GetAllTickets()
        {
            var response = await fixture.Client.GetAsync(new Uri($"{fixture.Client.BaseAddress}ticket")).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            Assert.AreEqual("[]", await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
    }
}
