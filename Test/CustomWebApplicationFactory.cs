using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lottery;
using Lottery.Models;

namespace Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        public const string TestDbName = "InMemoryDbForTesting";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder
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
        }
    }
}