using System.Collections.Generic;
using Xunit;

namespace DnsCrypt.Blacklist.Tests
{
	public class DomainBlacklistTests
    {
        [Fact]
        public async void BuildTest()
        {
	        var whitelist = new List<string>
	        {
		        "a-msedge.net",
		        "amazon.com",
		        "appsflyer.com",
		        "azurewebsites.net",
		        "cdnetworks.com",
		        "cloudapp.net",
		        "edgekey.net",
		        "elasticbeanstalk.com",
		        "github.com",
		        "github.io",
		        "invalid",
		        "j.mp",
		        "l-msedge.net",
		        "lan",
		        "localdomain",
		        "microsoft.com",
		        "msedge.net",
		        "nsatc.net",
		        "ovh.net",
		        "pusher.com",
		        "pusherapp.com",
		        "spotify.com",
		        "tagcommander.com",
		        "tracker.debian.org",
		        "windows.net"
			};

			var blacklists = new List<string>
	        {
				"file:Testfiles/domains-blacklist-local-additions.txt",
		        "http://osint.bambenekconsulting.com/feeds/c2-dommasterlist.txt",
		        "http://hosts-file.net/.%5Cad_servers.txt",
		        "http://mirror1.malwaredomains.com/files/justdomains",
		        "http://ransomwaretracker.abuse.ch/downloads/RW_DOMBL.txt",
		        "http://www.malwaredomainlist.com/mdlcsv.php?inactive=off",
		        "https://easylist-downloads.adblockplus.org/antiadblockfilters.txt",
		        "https://easylist-downloads.adblockplus.org/easylist_noelemhide.txt",
		        "https://easylist-downloads.adblockplus.org/easylistchina.txt",
		        "https://easylist-downloads.adblockplus.org/fanboy-social.txt",
		        "https://pgl.yoyo.org/adservers/serverlist.php",
		        "https://raw.githubusercontent.com/Dawsey21/Lists/master/adblock-list.txt",
		        "https://raw.githubusercontent.com/cjx82630/cjxlist/master/cjxlist.txt",
		        "https://raw.githubusercontent.com/liamja/Prebake/master/obtrusive.txt",
		        "https://s3.amazonaws.com/lists.disconnect.me/simple_malvertising.txt",
		        "https://s3.amazonaws.com/lists.disconnect.me/simple_malware.txt",
		        "https://s3.amazonaws.com/lists.disconnect.me/simple_tracking.txt",
		        "https://raw.githubusercontent.com/quidsup/notrack/master/trackers.txt",
		        "http://sysctl.org/cameleon/hosts",
		        "https://raw.githubusercontent.com/azet12/KADhosts/master/KADhosts.txt",
		        "https://raw.githubusercontent.com/marktron/fakenews/master/fakenews",
		        "http://mirror1.malwaredomains.com/files/dynamic_dns.txt",
		        "https://raw.githubusercontent.com/Clefspeare13/pornhosts/master/0.0.0.0/hosts",
		        "https://raw.githubusercontent.com/Sinfonietta/hostfiles/master/pornography-hosts",
		        "http://securemecca.com/Downloads/hosts.txt",
		        "https://raw.githubusercontent.com/Sinfonietta/hostfiles/master/gambling-hosts",
		        "https://raw.githubusercontent.com/Sinfonietta/hostfiles/master/social-hosts"
			};

			var result = await DomainBlacklist.Build(blacklists, whitelist);
			Assert.NotNull(result);
			Assert.Contains("eth0.me", result);
		}
    }
}
