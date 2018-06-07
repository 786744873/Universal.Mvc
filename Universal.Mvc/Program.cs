using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Universal.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                BuildWebHost(args).Run();

            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()//支持IIS宿主
                .UseKestrel()//支持自宿主
                .UseUrls("http://localhost:6003/")
                //设置 ContentRoot, ContentRoot是任何资源的根路径，比如页面和静态资源
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json",optional:true,reloadOnChange:true)//可以添加自定义的json文件
                    .AddEnvironmentVariables()//添加环境变量信息
                    .Build())
                .UseStartup<Startup>()
                .ConfigureLogging(logging => {
                    logging.ClearProviders();//清除掉原来的日志信息
                    logging.SetMinimumLevel(LogLevel.Trace);//设置日志等级
                    logging.AddConsole(options => options.IncludeScopes = true);
                })
                .UseNLog()//使用NLog
                .Build();
        }
    }
}
