using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nett;
using Xunit;
using Xunit.Abstractions;

namespace DnsCrypt.Configuration.Tests
{
	public class ConfigTests
	{
		private readonly ITestOutputHelper _output;

		public ConfigTests(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void Test_2_0_42()
		{
			const string version = "2.0.42";
			_output.WriteLine($"Running {version}");
			Assert.True(TestConfiguration(version));
		}

		[Fact]
		public void Test_2_0_41()
		{
			const string version = "2.0.41";
			_output.WriteLine($"Running {version}");
			Assert.True(TestConfiguration(version));
		}

		[Fact]
		public void Test_2_0_36()
		{
			const string version = "2.0.36";
			_output.WriteLine($"Running {version}");
			Assert.True(TestConfiguration(version));
		}

		private bool TestConfiguration(string version)
		{
			const string configName = "dnscrypt-proxy.toml";

			var deleteOnEveryRun = new List<string>
			{
				"public-resolvers.md", "public-resolvers.md.minisig", "relays.md", "relays.md.minisig"
			};


			var fullConfigFile = Path.Combine("Data", $"{version}-full-{configName}");
			var exampleConfigFile = Path.Combine("Data", $"{version}-example-{configName}");
			var exe = Path.Combine("Data", $"{version}-dnscrypt-proxy.exe");

			Assert.True(File.Exists(fullConfigFile));
			Assert.True(File.Exists(exampleConfigFile));
			Assert.True(File.Exists(exe));

			var settings = TomlSettings.Create(s => s.ConfigurePropertyMapping(m => m.UseTargetPropertySelector(standardSelectors => standardSelectors.IgnoreCase)));

			var fullConfigOriginal = Toml.ReadFile(fullConfigFile, settings);
			var fullConfigSerialized = Toml.ReadFile<DnscryptProxyConfiguration>(fullConfigFile, settings);

			var exampleConfigOriginal = Toml.ReadFile(exampleConfigFile, settings);
			var exampleConfigSerialized = Toml.ReadFile<DnscryptProxyConfiguration>(exampleConfigFile, settings);

			//quick check
			Assert.Equal(250, fullConfigSerialized.max_clients);
			Assert.Equal(250, exampleConfigSerialized.max_clients);

			_output.WriteLine($"Found {fullConfigOriginal.Keys.Count} keys in full original configuration");
			_output.WriteLine($"Found {exampleConfigOriginal.Keys.Count} keys in example original configuration");

			var tmpFullFile = Path.Combine("Data", $"tmp-{version}-full-{configName}");
			var tmpExampleFile = Path.Combine("Data", $"tmp-{version}-example-{configName}");
			Toml.WriteFile(fullConfigSerialized, tmpFullFile);

			Toml.WriteFile(exampleConfigSerialized, tmpExampleFile);
			var configFullTmp = Toml.ReadFile(tmpFullFile, settings);
			var configExampleTmp = Toml.ReadFile(tmpExampleFile, settings);

			var notInOriginalFull = configFullTmp.Keys.Except(fullConfigOriginal.Keys);
			Assert.Empty(notInOriginalFull);

			var notInOriginalExample = configExampleTmp.Keys.Except(exampleConfigOriginal.Keys);
			Assert.Empty(notInOriginalExample);

			var notInSerializedFull = fullConfigOriginal.Keys.Except(configFullTmp.Keys);
			Assert.Empty(notInSerializedFull);

			var notInSerializedExample = exampleConfigOriginal.Keys.Except(configExampleTmp.Keys);
			Assert.Empty(notInSerializedExample);

			Assert.Equal(fullConfigOriginal.Keys.Count, configFullTmp.Keys.Count);
			Assert.Equal(exampleConfigOriginal.Keys.Count, configExampleTmp.Keys.Count);

			//change full config to get some output
			fullConfigSerialized.use_syslog = false;
			fullConfigSerialized.log_file = null;
			fullConfigSerialized.log_level = 0;
			fullConfigSerialized.proxy = null;
			fullConfigSerialized.http_proxy = null;
			Toml.WriteFile(fullConfigSerialized, tmpFullFile);

			var processFullResult = Helper.RunProcess(exe, tmpFullFile);

			Assert.Contains("[NOTICE] Configuration successfully checked", processFullResult.StandardError);
			Assert.Equal(0, processFullResult.ExitCode);

			foreach (var fileToDelete in deleteOnEveryRun)
			{
				File.Delete(Path.Combine("Data", fileToDelete));
				Assert.False(File.Exists(Path.Combine("Data", fileToDelete)));
			}

			var processExampleResult = Helper.RunProcess(exe, tmpExampleFile);

			Assert.Contains("[NOTICE] Configuration successfully checked", processExampleResult.StandardError);
			Assert.Equal(0, processExampleResult.ExitCode);

			foreach (var fileToDelete in deleteOnEveryRun)
			{
				File.Delete(Path.Combine("Data", fileToDelete));
				Assert.False(File.Exists(Path.Combine("Data", fileToDelete)));
			}

			File.Delete(tmpFullFile);
			File.Delete(tmpExampleFile);

			return true;
		}
	}
}
