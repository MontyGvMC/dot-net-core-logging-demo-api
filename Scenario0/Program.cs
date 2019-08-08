using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace DotNetCoreLoggingDemoAPI.Scenario0
{

    /// <summary>
    /// Our programm.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The command line arguments to be used.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the web host builder.
        /// </summary>
        /// <param name="args">The command line arguments to be used.</param>
        /// <returns>Returns the IWebHostBuilder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging
                (
                    (context, loggingBuilder) =>
                    {
                    }
                )
                .UseNLog();

    }

}
