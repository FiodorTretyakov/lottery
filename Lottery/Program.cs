using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Lottery
{
    public static class Program
    {
        public static async Task Main() => await WebHost.CreateDefaultBuilder()
            .UseStartup<Startup>().Build().RunAsync().ConfigureAwait(false);
    }
}
