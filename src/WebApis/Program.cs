using System;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebApiSwc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ViewVersionOnConsole();
            BuildWebHost(args).Run();
        }


        public static void ViewVersionOnConsole()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            const string version = "CWS Ver2.9";
            Console.Title = version;
            Console.WriteLine($"{version}  [16.08.2019]  [Add TODO: for Cash HandleInvoke result in MiddlewareInvokeService]");
        }


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseHealthChecks("/")
                .ConfigureServices(services => services.AddAutofac())
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();     
    }
}
