using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnsCrypt.Models;
using DnsCrypt.Tools;
using DNS.Client;
using DNS.Protocol;
using NSec.Cryptography;

namespace DnsCrypt.Measurement
{
    public static class MeasurementTools
    {
	    public static async Task<MeasurementResult> Proxy(Stamp stamp)
	    {
		    if (stamp == null) return null;
		    var measurement = new MeasurementResult { Stamp = stamp};

		    try
		    {
				var request = new ClientRequest(stamp.Address, stamp.Port);
				request.Questions.Add(new Question(Domain.FromString(stamp.ProviderName), RecordType.TXT));
			    request.RecursionDesired = true;
			    var sw = Stopwatch.StartNew();
			    var response = await request.Resolve().ConfigureAwait(false);
			    sw.Stop();
			    if (response != null)
			    {
				    foreach (var answerRecord in response.AnswerRecords)
				    {
					    var certificates = new List<Certificate>();
					    var tr = Encoding.ASCII.GetString(ArrayHelper.SubArray(answerRecord.Data, 0, 9));
					    if (tr.Equals("|DNSC\0\u0001\0\0") || tr.Equals("|DNSC\0\u0002\0\0"))
					    {
						    var certificate = ExtractCertificate(ArrayHelper.SubArray(answerRecord.Data, 9),
							    Converters.StringToByteArray(stamp.PublicKey));
						    if (certificate != null)
						    {
							    if (certificate.Valid)
							    {
								    certificates.Add(certificate);
							    }
						    }
					    }

					    if (certificates.Count > 0)
					    {
						    var newestCertificate = certificates.OrderByDescending(item => item.Serial).FirstOrDefault();
						    if (newestCertificate != null)
						    {
							    measurement.Certificate = newestCertificate;
							    measurement.Failed = false;
						    }
						    else
						    {
							    measurement.Failed = true;
						    }
					    }
					    else
					    {
						    measurement.Failed = true;
					    }
				    }

				    measurement.Time = sw.ElapsedMilliseconds;
			    }
		    }
		    catch (Exception)
		    {
			    measurement.Failed = true;
		    }
		    return measurement;
	    }

	    private static Certificate ExtractCertificate(byte[] data, byte[] providerKey)
	    {
		    var certificate = new Certificate();
		    if (data.Length != 116) return null;
		    certificate.MagicQuery = ArrayHelper.SubArray(data, 96, 8);
		    var serial = ArrayHelper.SubArray(data, 104, 4);
		    var tsBegin = ArrayHelper.SubArray(data, 108, 4);
		    var tsEnd = ArrayHelper.SubArray(data, 112, 4);

		    if (BitConverter.IsLittleEndian)
		    {
			    Array.Reverse(serial);
			    Array.Reverse(tsBegin);
			    Array.Reverse(tsEnd);
		    }
		    certificate.Serial = BitConverter.ToInt32(serial, 0);
		    certificate.TsBegin = UnixTimeStampToDateTime(BitConverter.ToInt32(tsBegin, 0));
		    certificate.TsEnd = UnixTimeStampToDateTime(BitConverter.ToInt32(tsEnd, 0));

		    try
		    {
			    var algorithm = new Ed25519();
			    var publicKey = PublicKey.Import(algorithm, providerKey, KeyBlobFormat.RawPublicKey);
			    var signature = ArrayHelper.SubArray(data, 0, 64);
			    certificate.Valid = algorithm.TryVerify(publicKey, ArrayHelper.SubArray(data, 64), signature);
			    return certificate;
		    }
		    catch (Exception)
		    {
		    }
		    return null;
	    }

	    private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
	    {
		    var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		    dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
		    return dateTime;
	    }
	}
}
