using DnsCrypt.Models;
using DnsCrypt.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

				if (stampBinary[0] == 0x00)
				{
					stampObject.Protocol = StampProtocol.Plain;
				}
				else if (stampBinary[0] == 0x01)
				{
					stampObject.Protocol = StampProtocol.DnsCrypt;
				}
				else if (stampBinary[0] == 0x02)
				{
					stampObject.Protocol = StampProtocol.DoH;
				}
				else if (stampBinary[0] == 0x03)
				{
					stampObject.Protocol = StampProtocol.TLS;
				}
				else if (stampBinary[0] == 0x81)
				{
					stampObject.Protocol = StampProtocol.DNSCryptRelay;
				}

				switch (stampObject.Protocol)
				{
					case StampProtocol.Plain:
						break;
					case StampProtocol.DnsCrypt:
						if (stampBinary.Length < 66)
						{
							//Stamp is too short
							return null;
						}

						var dnsCryptProperties = stampBinary[1];
						stampObject.Properties.DnsSec = Convert.ToBoolean((dnsCryptProperties >> 0) & 1);
						stampObject.Properties.NoLog = Convert.ToBoolean((dnsCryptProperties >> 1) & 1);
						stampObject.Properties.NoFilter = Convert.ToBoolean((dnsCryptProperties >> 2) & 1);
						var dnsCryptCounter = 9;
						var dnsCryptAddressLength = stampBinary[dnsCryptCounter++];
						stampObject.Address = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, dnsCryptCounter, dnsCryptAddressLength));
						dnsCryptCounter += dnsCryptAddressLength;

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
						var publicKeyLength = stampBinary[dnsCryptCounter++];
						stampObject.PublicKey = Converters.ByteArrayToHexString(ArrayHelper.SubArray(stampBinary, dnsCryptCounter, publicKeyLength));
						dnsCryptCounter += publicKeyLength;
						var providerNameLength = stampBinary[dnsCryptCounter++];
						stampObject.ProviderName = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, dnsCryptCounter, providerNameLength));
						break;
					case StampProtocol.DoH:
						if (stampBinary.Length < 22)
						{
							//Stamp is too short
							return null;
						}

						var dohProperties = stampBinary[1];
						stampObject.Properties.DnsSec = Convert.ToBoolean((dohProperties >> 0) & 1);
						stampObject.Properties.NoLog = Convert.ToBoolean((dohProperties >> 1) & 1);
						stampObject.Properties.NoFilter = Convert.ToBoolean((dohProperties >> 2) & 1);
						var dohCounter = 9;
						var dohAddressLength = stampBinary[dohCounter++];
						stampObject.Address = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, dohCounter, dohAddressLength));
						dohCounter += dohAddressLength;

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
						var hashLength = stampBinary[dohCounter++];
						stampObject.Hash = Converters.ByteArrayToHexString(ArrayHelper.SubArray(stampBinary, dohCounter, hashLength));
						dohCounter += hashLength;
						var hostNameLength = stampBinary[dohCounter++];
						stampObject.Hostname = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, dohCounter, hostNameLength));
						dohCounter += hostNameLength;
						var pathLength = stampBinary[dohCounter++];
						stampObject.Path = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, dohCounter, pathLength));
						break;
					case StampProtocol.DNSCryptRelay:
						if (stampBinary.Length < 13)
						{
							//Stamp is too short
							return null;
						}
						var relayCounter = 1;
						var relayAddressLength = stampBinary[relayCounter++];
						stampObject.Address = Encoding.UTF8.GetString(ArrayHelper.SubArray(stampBinary, relayCounter, relayAddressLength));
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
							return null;
						}
						break;
					case StampProtocol.TLS:
						break;
					case StampProtocol.Unknown:
						break;
				}
				return stampObject;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static List<StampFileEntry> ReadStampFileEntries(string stampFilePath)
		{
			var stampList = new List<StampFileEntry>();
			if (!File.Exists(stampFilePath)) return stampList;
			var content = File.ReadAllText(stampFilePath);
			if (string.IsNullOrEmpty(content)) return stampList;
			var rawStampList = content.Split(new[] { '#', '#' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var rawStampListEntry in rawStampList)
			{
				var def = rawStampListEntry.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

				var stampFileEntry = new StampFileEntry();
				Stamp stamp = null;
				for (int i = 0; i < def.Length; i++)
				{
					if (i == 0)
					{
						stampFileEntry.Name = def[i].Trim();
					}
					if (def[i].StartsWith("sdns://"))
					{
						stamp = Decode(def[i].Trim());
					}
					else
					{
						if (i != 0)
						{
							stampFileEntry.Description += def[i];
						}
					}
				}

				if (stamp != null)
				{
					stampFileEntry.Stamp = stamp;
					stampList.Add(stampFileEntry);
				}
			}
			return stampList;
		}

		public static List<Stamp> ReadStampFile(string stampFilePath, bool noLog = false, bool noFilter = false, bool onlyDnsSec = false)
		{
			var stampList = new List<Stamp>();
			if (!File.Exists(stampFilePath)) return stampList;
			var content = File.ReadAllText(stampFilePath);
			if (string.IsNullOrEmpty(content)) return stampList;
			var rawStampList = content.Split(new[] { '#', '#' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var rawStampListEntry in rawStampList)
			{
				var def = rawStampListEntry.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

				Stamp stamp = null;
				for (int i = 0; i < def.Length; i++)
				{
					if (def[i].StartsWith("sdns://"))
					{
						stamp = Decode(def[i].Trim());
					}
				}

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
