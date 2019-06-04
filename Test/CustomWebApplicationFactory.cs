using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lottery;
using Lottery.Models;
using Microsoft.Extensions.Configuration;

namespace Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureServices(services =>
            {
                services
                    .AddEntityFrameworkSqlite()
                    .AddDbContext<TicketContext>(options =>
                    {
                        options.UseSqlite("DataSource=:memory:");
                        options.UseInternalServiceProvider(services.BuildServiceProvider());
                    });
            });
    }
}