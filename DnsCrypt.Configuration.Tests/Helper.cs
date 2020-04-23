using System.Diagnostics;
using System.Text;
using System.Threading;
using Xunit;

namespace DnsCrypt.Configuration.Tests
{
	public static class Helper
	{
		public static ProcessResult RunProcess(string exe, string config)
		{
			const int timeout = 5000;
			using var process = new Process
			{
				StartInfo =
				{
					FileName = exe,
					Arguments = $"-check -config {config}",
					UseShellExecute = false,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};
			var output = new StringBuilder();
			var error = new StringBuilder();

			using var outputWaitHandle = new AutoResetEvent(false);
			using var errorWaitHandle = new AutoResetEvent(false);

			process.OutputDataReceived += (sender, e) =>
			{
				if (e.Data == null)
				{
					outputWaitHandle.Set();
				}
				else
				{
					output.AppendLine(e.Data);
				}
			};
			process.ErrorDataReceived += (sender, e) =>
			{
				if (e.Data == null)
				{
					errorWaitHandle.Set();
				}
				else
				{
					error.AppendLine(e.Data);
				}
			};
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			Assert.True(process.WaitForExit(timeout));
			Assert.True(outputWaitHandle.WaitOne(timeout));
			Assert.True(errorWaitHandle.WaitOne(timeout));

			return new ProcessResult
			{
				ExitCode = process.ExitCode,
				StandardError = error.ToString(),
				StandardOutput = output.ToString()
			};
		}

	}
}
