using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Test
{
    public sealed class TestServerFixture : IDisposable
    {
        private readonly TestServer testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            testServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development").UseStartup<TestStartup>());
            Client = testServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            testServer.Dispose();
        }
    }
}