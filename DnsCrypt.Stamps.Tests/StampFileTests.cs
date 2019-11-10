using System.IO;
using System.Linq;
using Xunit;

namespace DnsCrypt.Stamps.Tests
{
	public class StampFileTests
	{
		[Fact]
		public void StampReadFileTest()
		{
			var testFile = Path.Combine("Testfiles", "public-resolvers.md");
			var stamps = StampTools.ReadStampFile(testFile);
			Assert.Equal(192, stamps.Count);
			Assert.Equal(64, stamps.Count(s => s.Protocol == Models.StampProtocol.DoH));
			Assert.Equal(128, stamps.Count(s => s.Protocol == Models.StampProtocol.DnsCrypt));
		}

		[Fact]
		public void StampReadRelayFileTest()
		{
			var testFile = Path.Combine("Testfiles", "relays.md");
			var stamps = StampTools.ReadStampFile(testFile);
			Assert.Equal(44, stamps.Count(s => s.Protocol == Models.StampProtocol.DNSCryptRelay));
		}
	}
}
