using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
namespace RedisServer
{
    public class Program
    {
        static Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            try { 
            
                Logger.Debug("Application Starting Up");

                //CreateHostBuilder(args).Build().Run(); 
                WebHost.CreateDefaultBuilder()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5050")
                .ConfigureKestrel(serverOptions =>
                {
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()
                .Build()
                .Run();
            }
            catch (Exception Exception)
            { 
                Logger.Error(Exception, "Stopped program because of exception");
                throw;
            } 
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>(); 
        //        })
        //        .ConfigureLogging(logging =>
        //        {
        //            logging.ClearProviders();
        //            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //        })
        //        .UseNLog();
    }
}
