namespace DnsCrypt.Models
{
	public class Measurement
	{
		public Stamp Stamp { get; set; }
		public long Time { get; set; }
		public bool Failed { get; set; }
		public Certificate Certificate { get; set; }
	}
}
