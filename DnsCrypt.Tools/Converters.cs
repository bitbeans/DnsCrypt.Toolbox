using System;
using System.Linq;

namespace DnsCrypt.Tools
{
	public static class Converters
    {
	    public static byte[] StringToByteArray(string hex)
	    {
		    return Enumerable.Range(0, hex.Length)
			    .Where(x => x % 2 == 0)
			    .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
			    .ToArray();
	    }

	    public static string ByteArrayToHexString(byte[] data)
	    {
		    var hexString = BitConverter.ToString(data);
		    return hexString.Replace("-", "").ToLower();
	    }
	}
}
