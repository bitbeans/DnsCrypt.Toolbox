using System;

namespace DnsCrypt.Models
{
	public class Certificate
	{
		public byte[] MagicQuery { get; set; }
		public int Serial { get; set; }
		public DateTime TsBegin { get; set; }
		public DateTime TsEnd { get; set; }
		public bool Valid { get; set; }
	}
}
