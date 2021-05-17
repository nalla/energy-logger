using System;
using System.ComponentModel.DataAnnotations;

namespace Energy.Logger.Shared.Models
{
	public class MeasurementDto
	{
		public double? Water { get; set; }

		public double? Electricity { get; set; }

		public double? Gas { get; set; }

		[Required]
		public DateTime Timestamp { get; set; }

		[Required]
		public string ApiKey { get; set; }
	}
}
