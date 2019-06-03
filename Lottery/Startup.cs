using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Lottery.Models;

namespace Lottery
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        [Obsolete("Until .net core 3")]
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        public IConfiguration Configuration { get; }

        [Obsolete("Until .net core 3")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<TicketContext>
                (options => options.UseLoggerFactory(MyLoggerFactory)
                .UseSqlite(Configuration.GetConnectionString("LotteryDatabase")));
        }
        
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
