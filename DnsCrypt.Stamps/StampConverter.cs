using System;
using System.Text;
using DnsCrypt.Models;
using DnsCrypt.Tools;

namespace DnsCrypt.Stamps
{
    public static class StampConverter
	{
		/// <summary>
		/// Decode an encoded Stamp. 
		/// </summary>
		/// <param name="stamp"></param>
		/// <returns>Stamp object.</returns>
		public static Stamp Decode(string stamp)
		{
			if (!stamp.Substring(0, 7).Equals("sdns://"))
			{
				return null;
			}

			var stampObject = new Stamp
			{
				Protocol = StampProtocol.Unknown,
				Properties = new StampProperties()
			};
			var stampBinary = Base64Url.Decode(stamp.Substring(7));

			if (stampBinary[0] == 0x01)
			{
				stampObject.Protocol = StampProtocol.DnsCrypt;
			}
			else if (stampBinary[0] == 0x02)
			{
				stampObject.Protocol = StampProtocol.DoH;
			}

			var properties = stampBinary[1];
			stampObject.Properties.DnsSec = Convert.ToBoolean((properties >> 0) & 1);
			stampObject.Properties.NoLog = Convert.ToBoolean((properties >> 1) & 1);
			stampObject.Properties.NoFilter = Convert.ToBoolean((properties >> 2) & 1);
			var i = 9;
			var addressLength = stampBinary[i++];
			stampObject.Address = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, i, addressLength));
			i += addressLength;

			switch (stampObject.Protocol)
			{
				case StampProtocol.DnsCrypt:
					var publicKeyLength = stampBinary[i++];
					stampObject.PublicKey = Converters.ByteArrayToHexString(ArrayHelper.SubArray(stampBinary, i, publicKeyLength));
					i += publicKeyLength;
					var providerNameLength = stampBinary[i++];
					stampObject.ProviderName = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, i, providerNameLength));
					break;
				case StampProtocol.DoH:
					var hashLength = stampBinary[i++];
					stampObject.Hash = Converters.ByteArrayToHexString(ArrayHelper.SubArray(stampBinary, i, hashLength));
					i += hashLength;
					var hostNameLength = stampBinary[i++];
					stampObject.Hostname = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, i, hostNameLength));
					i += hostNameLength;
					var pathLength = stampBinary[i++];
					stampObject.Path = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, i, pathLength));
					break;
				default:
					stampObject = null;
					break;
			}
			return stampObject;
		}

		
	}
}
