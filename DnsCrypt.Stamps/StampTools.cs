using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DnsCrypt.Models;
using DnsCrypt.Tools;

namespace DnsCrypt.Stamps
{
    public static class StampTools
	{
		/// <summary>
		/// Decode an encoded Stamp. 
		/// </summary>
		/// <param name="stamp"></param>
		/// <returns>Stamp object.</returns>
		public static Stamp Decode(string stamp)
		{
			try
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

				if (!string.IsNullOrEmpty(stampObject.Address))
				{
					if (Uri.TryCreate(string.Format("http://{0}", stampObject.Address), UriKind.Absolute, out Uri url))
					{
						stampObject.Address = url.Host;
						if (url.Port == 80)
						{
							stampObject.Port = 443;
						}
						else
						{
							stampObject.Port = url.Port;
						}
					}
				}
				else
				{
					//eg: google
					stampObject.Port = 53; 
				}

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
			catch (Exception)
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stampFilePath"></param>
		/// <param name="noLog"></param>
		/// <param name="noFilter"></param>
		/// <param name="onlyDnsSec"></param>
		/// <returns></returns>
		public static List<Stamp> ReadStampFile(string stampFilePath, bool noLog = false, bool noFilter = false, bool onlyDnsSec = false)
		{
			var stampList = new List<Stamp>();
			if (!File.Exists(stampFilePath)) return stampList;
			var content = File.ReadAllText(stampFilePath);
			if (string.IsNullOrEmpty(content)) return stampList;
			var rawStampList = content.Split(new[] { '#', '#' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var rawStampListEntry in rawStampList)
			{
				var def = rawStampListEntry.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
				if (def.Length != 3) continue;

				var stamp = Decode(def[2].Trim());
				if (stamp != null)
				{
					
					if (onlyDnsSec)
					{
						if (!stamp.Properties.DnsSec)
						{
							continue;
						}
					}
					if (noFilter)
					{
						if (!stamp.Properties.NoFilter)
						{
							continue;
						}
					}
					if (noLog)
					{
						if (!stamp.Properties.NoLog)
						{
							continue;
						}
					}

					stampList.Add(stamp);
				}
			}
			return stampList;
		}
	}
}
