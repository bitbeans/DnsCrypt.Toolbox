using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class Whitelist : PropertyChangedBase
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

		private string _log_format;
		private string _whitelist_file;
		private string _log_file;

		/// <summary>
		/// Path to the file of whitelisting rules (absolute, or relative to the same directory as the config file)
		/// </summary>
		[TomlComment("# Path to the file of whitelisting rules (absolute, or relative to the same directory as the config file)", CommentLocation.Prepend)]
		public string whitelist_file
		{
			get => _whitelist_file;
			set => SetAndNotify(ref _whitelist_file, value);
		}

		/// <summary>
		/// Optional path to a file logging whitelisted queries
		/// </summary>
		[TomlComment("# Optional path to a file logging whitelisted queries", CommentLocation.Prepend)]
		public string log_file
		{
			get => _log_file;
			set => SetAndNotify(ref _log_file, value);
		}

		/// <summary>
		/// Optional log format: tsv or ltsv (default: tsv)
		/// Log format (SimpleDnsCrypt: ltsv).
		/// </summary>
		[TomlComment("# Optional log format: tsv or ltsv (default: tsv)", CommentLocation.Prepend)]
		[TomlComment("# Log format (SimpleDnsCrypt: ltsv).", CommentLocation.Prepend)]
		public string log_format
		{
			get => _log_format;
			set => SetAndNotify(ref _log_format, value);
		}
	}
}
