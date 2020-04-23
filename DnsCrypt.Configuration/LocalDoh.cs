using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class LocalDoh : PropertyChangedBase
	{
		#region Ignore for serialization
		private Action<Action> _propertyChangedDispatcher = Execute.DefaultPropertyChangedDispatcher;

		[TomlIgnore]
		[XmlIgnore]
		public virtual Action<Action> PropertyChangedDispatcher
		{
			get => _propertyChangedDispatcher;
			set => _propertyChangedDispatcher = value;
		}

		/// <summary>
		/// Occurs when a property value changes
		/// </summary>
		[TomlIgnore]
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		private BindableCollection<string> _listen_addresses;
		private string _path;
		private string _cert_file;
		private string _cert_key_file;

		/// <summary>
		/// Addresses that the local DoH server should listen to
		/// </summary>
		[TomlComment("# Addresses that the local DoH server should listen to", CommentLocation.Prepend)]
		public BindableCollection<string> listen_addresses
		{
			get => _listen_addresses;
			set => SetAndNotify(ref _listen_addresses, value);
		}

		/// <summary>
		/// Path of the DoH URL. This is not a file, but the part after the hostname
		/// in the URL. By convention, `/dns-query` is frequently chosen.
		/// For each `listen_address` the complete URL to access the server will be:
		/// `https://<listen_address><path>` (ex: `https://127.0.0.1/dns-query`)
		/// </summary>
		[TomlComment("# Path of the DoH URL. This is not a file, but the part after the hostname", CommentLocation.Prepend)]
		[TomlComment("# in the URL. By convention, `/dns-query` is frequently chosen.", CommentLocation.Prepend)]
		[TomlComment("# For each `listen_address` the complete URL to access the server will be:", CommentLocation.Prepend)]
		[TomlComment("# `https://<listen_address><path>` (ex: `https://127.0.0.1/dns-query`)", CommentLocation.Prepend)]
		public string path
		{
			get => _path;
			set => SetAndNotify(ref _path, value);
		}

		/// <summary>
		/// Certificate file and key - Note that the certificate has to be trusted.
		/// See the documentation (wiki) for more information.
		/// </summary>
		[TomlComment("# Certificate file and key - Note that the certificate has to be trusted.", CommentLocation.Prepend)]
		[TomlComment("# See the documentation (wiki) for more information.", CommentLocation.Prepend)]
		public string cert_file
		{
			get => _cert_file;
			set => SetAndNotify(ref _cert_file, value);
		}

		public string cert_key_file
		{
			get => _cert_key_file;
			set => SetAndNotify(ref _cert_key_file, value);
		}

	}
}
