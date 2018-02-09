namespace DnsCrypt.Models
{
	public class Stamp
	{
		public StampProtocol Protocol { get; set; }
		public StampProperties Properties { get; set; }
		public string Address { get; set; }
		public string PublicKey { get; set; }
		public string ProviderName { get; set; }
		public string Hash { get; set; }
		public string Hostname { get; set; }
		public string Path { get; set; }
	}
}
