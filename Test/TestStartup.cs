using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Lottery;
using Lottery.Models;

namespace Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        [Obsolete("Until .net core 3")]
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<TicketContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                            options.UseInternalServiceProvider(new ServiceCollection()
                                .AddEntityFrameworkInMemoryDatabase()
                                .BuildServiceProvider());
                        });
        }
    }
}