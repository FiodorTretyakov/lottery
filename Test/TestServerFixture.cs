using System;
using System.Net.Http;
using Lottery;
using Lottery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Test
{
    public sealed class TestServerFixture : IDisposable
    {
        public const string TestDbName = "InMemoryDbForTesting";

        private readonly TestServer testServer;

        public HttpClient Client { get; }

        public TestServerFixture()
        {
            testServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development").UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<TicketContext>(options =>
                                {
                                    options.UseInMemoryDatabase(TestDbName);
                                    options.UseInternalServiceProvider(new ServiceCollection()
                                        .AddEntityFrameworkInMemoryDatabase()
                                        .BuildServiceProvider());
                                });
                }));
            Client = testServer.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            testServer.Dispose();
        }
    }
}