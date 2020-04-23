using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class TlsClientAuthCred : PropertyChangedBase
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

		private string _server_name;
		private string _client_cert;
		private string _client_key;

		public string server_name
		{
			get => _server_name;
			set => SetAndNotify(ref _server_name, value);
		}

		public string client_cert
		{
			get => _client_cert;
			set => SetAndNotify(ref _client_cert, value);
		}

		public string client_key
		{
			get => _client_key;
			set => SetAndNotify(ref _client_key, value);
		}
	}

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class TlsClientAuth : PropertyChangedBase
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

		private BindableCollection<TlsClientAuthCred> _creds;

		public BindableCollection<TlsClientAuthCred> creds
		{
			get => _creds;
			set => SetAndNotify(ref _creds, value);
		}
	}
}
