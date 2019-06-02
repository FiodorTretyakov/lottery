using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Lottery;
using System.Net.Http;

namespace Test
{
    public sealed class TestServerFixture : IDisposable
    {
        private readonly TestServer testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            testServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development").UseStartup<Startup>());
            Client = testServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            testServer.Dispose();
        }
    }
}