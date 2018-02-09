using DnsCrypt.Models;
using Xunit;

namespace DnsCrypt.Stamps.Tests
{
	public class StampDecodeTests
	{
        [Fact]
        public void StampDecodeTest1()
        {
	        const string stamp = "sdns://AQcAAAAAAAAADjIxMi40Ny4yMjguMTM2IOgBuE6mBr-wusDOQ0RbsV66ZLAvo8SqMa4QY2oHkDJNHzIuZG5zY3J5cHQtY2VydC5mci5kbnNjcnlwdC5vcmc";
			var result = StampConverter.Decode(stamp);
	        Assert.Equal(StampProtocol.DnsCrypt, result.Protocol);
			Assert.Equal("2.dnscrypt-cert.fr.dnscrypt.org", result.ProviderName);
		}

	    [Fact]
	    public void StampDecodeTest2()
	    {
		    const string stamp = "sdns://AgcAAAAAAAAADTM3LjU5LjIzOC4yMTMgwzRA_TfjYt0RwSHqBHwj7OM-D_x-CDgqIHeJHIoN1P0UZG9oLmZyLmRuc2NyeXB0LmluZm8KL2Rucy1xdWVyeQ";
		    var result = StampConverter.Decode(stamp);
		    Assert.Equal(StampProtocol.DoH, result.Protocol);
			Assert.Equal("doh.fr.dnscrypt.info", result.Hostname);
	    }

	    [Fact]
	    public void StampDecodeTest3()
	    {
		    const string stamp = "sdns://AgUAAAAAAAAAACDyXGrcc5eNecJ8nomJCJ-q6eCLTEn6bHic0hWGUwYQaA5kbnMuZ29vZ2xlLmNvbQ0vZXhwZXJpbWVudGFs";
		    var result = StampConverter.Decode(stamp);
		    Assert.Equal(StampProtocol.DoH, result.Protocol);
			Assert.Equal("dns.google.com", result.Hostname);
	    }
	}
}
