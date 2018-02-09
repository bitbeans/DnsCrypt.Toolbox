using System;

namespace DnsCrypt.Tools
{
	/// <summary>
	/// source: https://gist.github.com/igorushko/cccef0561aea7e46ae52bc62270b2b61
	/// </summary>
	public static class Base64Url
	{
		public static string Encode(byte[] arg)
		{
			if (arg == null) throw new ArgumentNullException("arg");

			var s = Convert.ToBase64String(arg);
			return s
				.Replace("=", "")
				.Replace("/", "_")
				.Replace("+", "-");
		}

		public static string ToBase64(string arg)
		{
			if (arg == null) throw new ArgumentNullException("arg");

			var s = arg
				.PadRight(arg.Length + (4 - arg.Length % 4) % 4, '=')
				.Replace("_", "/")
				.Replace("-", "+");

			return s;
		}

		public static byte[] Decode(string arg)
		{
			var decrypted = ToBase64(arg);

			return Convert.FromBase64String(decrypted);
		}
	}
}