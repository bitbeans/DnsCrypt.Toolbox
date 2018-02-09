using DnsCrypt.Stamps;
using Xunit;

namespace DnsCrypt.Measurement.Tests
{
	public class MeasurementTests
    {
        [Fact]
        public async void MeasureTest1()
        {
	        const string stamp = "sdns://AQcAAAAAAAAADjIxMi40Ny4yMjguMTM2IOgBuE6mBr-wusDOQ0RbsV66ZLAvo8SqMa4QY2oHkDJNHzIuZG5zY3J5cHQtY2VydC5mci5kbnNjcnlwdC5vcmc";
	        var result = StampConverter.Decode(stamp);
			var measurement = await Measure.Proxy(result).ConfigureAwait(false);
			Assert.NotNull(measurement);
			Assert.False(measurement.Failed);
        }
    }
}
