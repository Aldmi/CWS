using System;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
            string version = GetVersion();
            Console.Title = version;
            Console.WriteLine($"{version}");
        }


        public static string GetVersion()
        {
            const string version = "CWS Ver4.5";
            return $"{version}  [10.12.2019]  [Reliz. Add CrcCalc.Calc8Bit]";
        }


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();     
    }
}
