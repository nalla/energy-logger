using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Energy.Logger.Api
{
	public class Program
	{
		public static int Main( string[] args )
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			try
			{
				Log.Information("Starting web host");
				CreateHostBuilder(args).Build().Run();

				return 0;
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Host terminated unexpectedly");

				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>Host
			.CreateDefaultBuilder(args)
			.UseSerilog()
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
	}
}
