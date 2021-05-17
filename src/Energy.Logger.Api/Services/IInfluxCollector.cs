using Energy.Logger.Shared.Models;

namespace Energy.Logger.Api.Services
{
	public interface IInfluxCollector
	{
		void Record(MeasurementDto measurement);
	}
}
