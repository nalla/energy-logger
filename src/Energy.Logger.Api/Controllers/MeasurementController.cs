using Energy.Logger.Api.Services;
using Energy.Logger.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Energy.Logger.Api.Controllers
{
	[ApiController]
	[Route("measurements")]
	public class MeasurementController : ControllerBase
	{
		private readonly ILogger<MeasurementController> logger;
		private readonly IConfiguration configuration;
		private readonly IInfluxCollector influxCollector;

		public MeasurementController(ILogger<MeasurementController> logger, IConfiguration configuration, IInfluxCollector influxCollector)
		{
			this.logger = logger;
			this.configuration = configuration;
			this.influxCollector = influxCollector;
		}

		[HttpPost]
		public IActionResult Record(MeasurementDto measurement)
		{
			if (!measurement.Water.HasValue && !measurement.Electricity.HasValue && !measurement.Gas.HasValue)
			{
				return BadRequest("Invalid values provided!");
			}

			if (measurement.ApiKey?.Equals(configuration["apiKey"]) == true)
			{
				influxCollector.Record(measurement);
				logger.LogInformation($"Water: {measurement.Water}, Electicity: {measurement.Electricity}, Gas: {measurement.Gas}, Timestamp: {measurement.Timestamp}");

				return NoContent();
			}
			else
			{
				return BadRequest("Invalid api key provided!");
			}
		}
	}
}
