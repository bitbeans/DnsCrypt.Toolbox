using System.IO;
using Xunit;

namespace DnsCrypt.Stamps.Tests
{
	public class StampFileTests
    {
	    [Fact]
	    public void StampReadFileTest()
	    {
		    var testFile = Path.Combine("Testfiles", "public-resolvers.md");
		    var stamps = StampConverter.ReadStampFile(testFile);
			Assert.Equal(81, stamps.Count);
	    }
	}
}
