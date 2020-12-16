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
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException; ;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ViewVersionOnConsole();
            BuildWebHost(args).Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
            Console.WriteLine("MyHandler caught : " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
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
            const string version = "CWS Ver7.28";
            return $"{version}  [02.12.2020]  [Relise. Add CreepingLineRunningConvertert]";
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();     
    }
}
