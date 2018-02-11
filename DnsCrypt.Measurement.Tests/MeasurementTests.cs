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
	        var result = StampTools.Decode(stamp);
			var measurement = await MeasurementTools.Proxy(result).ConfigureAwait(false);
			Assert.NotNull(measurement);
			Assert.False(measurement.Failed);
        }

		[Fact]
		public async void MeasureTest2()
		{
			const string stamp = "sdns://AQcAAAAAAAAAEzIwOS4yNTAuMjM1LjE3MDo0NDMgz0wbvISl_NVCSe0wDJMS79BAFZoWth1djmhuzv_n3KAiMi5kbnNjcnlwdC1jZXJ0LmRlLmRuc21hc2NoaW5lLm5ldA";
			var result = StampTools.Decode(stamp);
			var measurement = await MeasurementTools.Proxy(result).ConfigureAwait(false);
			Assert.NotNull(measurement);
			Assert.False(measurement.Failed);
		}
	}
}
