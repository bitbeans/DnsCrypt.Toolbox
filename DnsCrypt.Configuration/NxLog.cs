using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	/// <summary>
	///     Log queries for nonexistent zones.
	/// </summary>
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class NxLog : PropertyChangedBase
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

		private string _file;
		private string _format;

		/// <summary>
		/// Path to the query log file (absolute, or relative to the same directory as the config file)
		/// </summary>
		[TomlComment("# Path to the query log file (absolute, or relative to the same directory as the config file)", CommentLocation.Prepend)]
		public string file
		{
			get => _file;
			set => SetAndNotify(ref _file, value);
		}

		/// <summary>
		/// Query log format (currently supported: tsv and ltsv)
		/// </summary>
		[TomlComment("# Query log format (currently supported: tsv and ltsv)", CommentLocation.Prepend)]
		public string format
		{
			get => _format;
			set => SetAndNotify(ref _format, value);
		}
	}
}
