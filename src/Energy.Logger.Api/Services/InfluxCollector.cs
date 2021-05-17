using Energy.Logger.Shared.Models;
using InfluxDB.Collector;
using InfluxDB.Collector.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Energy.Logger.Api.Services
{
	public class InfluxCollector : IInfluxCollector
	{
		public InfluxCollector(IConfiguration configuration, ILogger<InfluxCollector> logger)
		{
			var url = configuration.GetValue<string>("influxdb:url");
			var database = configuration.GetValue<string>("influxdb:database");

			Metrics.Collector = new CollectorConfiguration()
				.Batch.AtInterval(TimeSpan.FromSeconds(2))
				.WriteTo.InfluxDB(url, database)
				.CreateCollector();
			CollectorLog.RegisterErrorHandler((message, exception) => logger.LogError(exception, message));
		}

		public void Record(MeasurementDto measurement)
		{
			var values = new Dictionary<string, object>();

			if (measurement.Water.HasValue)
			{
				values.Add("water", measurement.Water.Value);
			}

			if (measurement.Electricity.HasValue)
			{
				values.Add("electricity", measurement.Electricity.Value);
			}

			if (measurement.Gas.HasValue)
			{
				values.Add("gas", measurement.Gas.Value);
			}

			Metrics.Collector.Write(
				"energy",
				values,
				timestamp: measurement.Timestamp.ToUniversalTime());
		}
	}
}
