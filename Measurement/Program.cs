using DnsCrypt.Measurement;
using DnsCrypt.Models;
using DnsCrypt.Stamps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mono.Options;
using Newtonsoft.Json;

namespace Measurement
{
	internal class Program
    {
	    private static async Task Main(string[] args)
	    {
		    var showHelp = false;
		    var list = "public-resolvers.md";
		    var noLogs = false;
		    var noFilter = false;
			var onlyDnssec = false;
		    var json = false;

			var p = new OptionSet
		    {
			    {
				    "l|list=", "the path to a resolvers.md file",
				    v =>
				    {
					    if (v != null) list = v;
				    }
			    },
			    {
				    "nl|nologs", "only test resolvers with NoLogs support enabled.",
				    v =>
				    {
					    if (v != null) noLogs = true;
				    }
			    },
			    {
				    "nf|nofilter", "only test resolvers with NoFilter support enabled.",
				    v =>
				    {
					    if (v != null) noFilter = true;
				    }
			    },
				{
				    "d|dnssec", "only test resolvers with DNSSEC support enabled.",
				    v =>
				    {
					    if (v != null) onlyDnssec = true;
				    }
			    },
			    {
				    "j|json", "print result as JSON.",
				    v =>
				    {
					    if (v != null) json = true;
				    }
			    },
				{
				    "h|help", "show this message and exit",
				    v => showHelp = v != null
			    }
		    };

		    try
		    {
			    p.Parse(args);
		    }
		    catch (OptionException e)
		    {
			    Console.Write("Measurement: ");
			    Console.WriteLine(e.Message);
			    Console.WriteLine("Try `Measurement --help' for more information.");
			    return;
		    }

		    if (showHelp)
		    {
			    ShowHelp(p);
			    return;
		    }


		    if (File.Exists(list))
		    {
			    var stamps = StampTools.ReadStampFile(list, noLogs, noFilter, onlyDnssec);
				var measurementResults = new List<MeasurementResult>();
			    if (!json)
			    {
				    Console.WriteLine("stay tuned . . .");
			    }

			    foreach (var stamp in stamps)
			    {
				    if (stamp.Protocol == StampProtocol.DnsCrypt)
				    {
					    var measurement = await MeasurementTools.Proxy(stamp).ConfigureAwait(false);
					    if (!measurement.Failed)
					    {
						    measurementResults.Add(measurement);
					    }
				    }
			    }
			    measurementResults.Sort((a, b) => a.Time.CompareTo(b.Time));
			    if (!json)
			    {
				    Console.WriteLine("=====================================");
				    Console.WriteLine($"{measurementResults.Count} Resolvers (fastest first)");
				    Console.WriteLine($"Only DNSSEC: {onlyDnssec}");
				    Console.WriteLine($"Only NoLogs: {noLogs}");
				    Console.WriteLine($"Only NoFilter: {noFilter}");
				    Console.WriteLine("=====================================");
				    foreach (var measurement in measurementResults)
				    {
					    if (!measurement.Failed)
					    {
						    Console.WriteLine(
							    $"{measurement.Time} ms, {measurement.Stamp.ProviderName}, " +
							    $"NoLogs: {measurement.Stamp.Properties.NoLog}, " +
							    $"NoFilter: {measurement.Stamp.Properties.NoFilter} " +
							    $"DNSSEC: {measurement.Stamp.Properties.DnsSec}, " +
							    $"Certificate Valid: {measurement.Certificate.Valid}");
					    }
				    }
			    }
			    else
			    {
					Console.WriteLine(JsonConvert.SerializeObject(measurementResults.Where(m => m.Failed == false).ToList()));
				}
		    }
		    else
		    {
			    Console.WriteLine("Missing resolvers.md");
		    }
		    Console.ReadLine();
	    }

	    private static void ShowHelp(OptionSet p)
	    {
		    Console.WriteLine("Usage: Measurement [OPTIONS]+");
		    Console.WriteLine();
		    Console.WriteLine("Options:");
		    p.WriteOptionDescriptions(Console.Out);
	    }
	}
}
